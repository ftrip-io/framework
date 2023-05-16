using ftrip.io.framework.Contexts;
using ftrip.io.framework.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.framework.Persistence.Sql.Database
{
    public abstract class DatabaseContextBase<T> : DbContext where T : DbContext
    {
        private readonly CurrentUserContext _currentUserContext;
        private static readonly HashSet<Type> _physicallyDeletableTypes = new HashSet<Type>();

        public DatabaseContextBase(DbContextOptions<T> options, CurrentUserContext currentUserContext) :
            base(options)
        {
            _currentUserContext = currentUserContext;

            ChangeTracker.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetFilterForSoftDeletedEntities(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SetFilterForSoftDeletedEntities(ModelBuilder modelBuilder)
        {
            foreach (var softDeletableTypeBuilder in modelBuilder.Model.GetEntityTypes()
                .Where(x => typeof(ISoftDeleteable).IsAssignableFrom(x.ClrType))
                .Where(x => !_physicallyDeletableTypes.Contains(x.ClrType)))
            {
                var parameter = Expression.Parameter(softDeletableTypeBuilder.ClrType, "p");

                softDeletableTypeBuilder.SetQueryFilter(
                    Expression.Lambda(
                        Expression.Equal(Expression.Property(parameter, nameof(ISoftDeleteable.Active)), Expression.Constant(true)),
                        parameter
                    )
                );
            }
        }

        public override int SaveChanges()
        {
            ModifyTrackedEntities();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ModifyTrackedEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ModifyTrackedEntities();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ModifyTrackedEntities();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ModifyTrackedEntities()
        {
            DetectChanges();
            SoftDeleteEntites();
            ModifyCreatedProperties();
            ModifyUpdatedProperties();
        }

        private void DetectChanges()
        {
            ChangeTracker.DetectChanges();
        }

        private void ModifyCreatedProperties()
        {
            var createdEntities = ChangeTracker.Entries<IAuditable>().Where(t => t.State == EntityState.Added);
            foreach (var entry in createdEntities)
            {
                var entity = entry.Entity;
                entity.CreatedAt = DateTime.UtcNow;
                entity.CreatedBy = _currentUserContext.Id;
                entity.ModifiedAt = DateTime.UtcNow;
                entity.ModifiedBy = _currentUserContext.Id;
            }
        }

        private void ModifyUpdatedProperties()
        {
            var modifiedEntities = ChangeTracker.Entries<IAuditable>().Where(t => t.State == EntityState.Modified);
            foreach (var entry in modifiedEntities)
            {
                var entity = entry.Entity;
                entity.ModifiedAt = DateTime.UtcNow;
                entity.ModifiedBy = _currentUserContext.Id;
            }
        }

        protected void IgnoreSoftDelete(Type type)
        {
            _physicallyDeletableTypes.Add(type);
        }

        private void SoftDeleteEntites()
        {
            var deletedEntities = ChangeTracker
                .Entries<ISoftDeleteable>()
                .Where(t => t.State == EntityState.Deleted)
                .Where(t => !_physicallyDeletableTypes.Contains(t.Entity.GetType()));

            foreach (var entry in deletedEntities)
            {
                var entity = entry.Entity;
                entity.Active = false;
                entry.State = EntityState.Modified;
                OnSoftDeleteEntity(entry);
            }
        }

        protected virtual void OnSoftDeleteEntity(EntityEntry entry)
        {
        }
    }
}