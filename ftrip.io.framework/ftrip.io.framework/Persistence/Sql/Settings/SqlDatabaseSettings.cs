namespace ftrip.io.framework.Persistence.Sql.Settings
{
    public class SqlDatabaseSettings
    {
        public virtual string Server { get; set; }
        public virtual string Port { get; set; }
        public virtual string Database { get; set; }
        public virtual string User { get; set; }
        public virtual string Password { get; set; }

        public string GetConnectionString()
        {
            return $"server={Server};port={Port};database={Database};user={User};password={Password}";
        }
    }
}