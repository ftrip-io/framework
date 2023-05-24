using ftrip.io.framework.Utilities;

namespace ftrip.io.framework.Tracing
{
    public class FromEnvJaegerSettings : JaegerSettings
    {
        public override string Server { get => EnvReader.GetEnvVariableOrThrow("JAEGER_SERVER"); }
        public override string Port { get => EnvReader.GetEnvVariableOrThrow("JAEGER_PORT"); }
    }
}