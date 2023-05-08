using ftrip.io.framework.Utilities;

namespace ftrip.io.framework.email.Settings
{
    public class FromEnvSMTPSettings : SMTPSettings
    {
        public override string Host { get => EnvReader.GetEnvVariableOrThrow("SMTP_HOST"); }
        public override int Port { get => int.Parse(EnvReader.GetEnvVariableOrThrow("SMTP_PORT")); }
        public override string Username { get => EnvReader.GetEnvVariableOrThrow("SMTP_USERNAME"); }
        public override string Password { get => EnvReader.GetEnvVariableOrThrow("SMTP_PASSWORD"); }
    }
}