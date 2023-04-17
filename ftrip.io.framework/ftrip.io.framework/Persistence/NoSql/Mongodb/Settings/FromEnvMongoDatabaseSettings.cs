using ftrip.io.framework.Utilities;

namespace ftrip.io.framework.Persistence.NoSql.Mongodb.Settings
{
    public class FromEnvMongoDatabaseSettings : MongoDatabaseSettings
    {
        public override string Url { get => EnvReader.GetEnvVariableOrThrow("MDB_URL"); }
        public override string Username { get => EnvReader.GetEnvVariableOrThrow("MDB_USERNAME"); }
        public override string Password { get => EnvReader.GetEnvVariableOrThrow("MDB_PASSWORD"); }
        public override string Database { get => EnvReader.GetEnvVariableOrThrow("MDB_DATABASE"); }
    }
}