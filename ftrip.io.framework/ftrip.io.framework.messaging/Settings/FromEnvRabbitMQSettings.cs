using ftrip.io.framework.Utilities;

namespace ftrip.io.framework.messaging.Settings
{
    public class FromEnvRabbitMQSettings : RabbitMQSettings
    {
        public override string Server { get => EnvReader.GetEnvVariableOrThrow("RMQ_SERVER"); }
        public override string Port { get => EnvReader.GetEnvVariableOrThrow("RMQ_PORT"); }
        public override string User { get => EnvReader.GetEnvVariableOrThrow("RMQ_USER"); }
        public override string Password { get => EnvReader.GetEnvVariableOrThrow("RMQ_PASSWORD"); }
    }
}