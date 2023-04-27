using ftrip.io.framework.Utilities;

namespace ftrip.io.framework.Secrets
{
    public class EnviromentSecretsManager : ISecretsManager
    {
        public EnviromentSecretsManager()
        {
        }

        public string Get(string key)
        {
            return EnvReader.GetEnvVariableOrThrow(key);
        }
    }
}