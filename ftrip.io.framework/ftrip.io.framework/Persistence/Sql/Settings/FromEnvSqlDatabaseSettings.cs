using ftrip.io.framework.Utilities;

namespace ftrip.io.framework.Persistence.Sql.Settings
{
    public class FromEnvSqlDatabaseSettings : SqlDatabaseSettings
    {
        public override string Server { get => EnvReader.GetEnvVariableOrThrow("DB_SERVER"); }
        public override string Port { get => EnvReader.GetEnvVariableOrThrow("DB_PORT"); }
        public override string Database { get => EnvReader.GetEnvVariableOrThrow("DB_DATABASE"); }
        public override string User { get => EnvReader.GetEnvVariableOrThrow("DB_USER"); }
        public override string Password { get => EnvReader.GetEnvVariableOrThrow("DB_PASSWORD"); }
    }
}