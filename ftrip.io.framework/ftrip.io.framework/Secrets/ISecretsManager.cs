namespace ftrip.io.framework.Secrets
{
    public interface ISecretsManager
    {
        string Get(string key);
    }
}