namespace ftrip.io.framework.Globalization
{
    public interface IStringManager
    {
        string GetString(string key);

        string Format(string key, params object[] args);

        string Format(string key, object arg0, object arg1, object arg2);

        string Format(string key, object arg0, object arg1);

        string Format(string key, object arg0);
    }
}