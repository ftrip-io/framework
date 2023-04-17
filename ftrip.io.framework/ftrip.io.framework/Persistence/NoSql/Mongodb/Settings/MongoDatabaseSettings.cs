namespace ftrip.io.framework.Persistence.NoSql.Mongodb.Settings
{
    public class MongoDatabaseSettings
    {
        public virtual string Url { get; set; }
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual string Database { get; set; }

        public string GetConnectionString()
        {
            return $"mongodb://{Username}:{Password}@{Url}";
        }
    }
}