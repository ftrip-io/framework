using ftrip.io.framework.Contexts;
using System.Globalization;
using System.Resources;

namespace ftrip.io.framework.Globalization
{
    public class StringManager : IStringManager
    {
        private readonly ResourceManager _resourceManager;
        private readonly GlobalizationContext _globalizationContext;

        public StringManager(ResourceManager resourceManager, GlobalizationContext globalizationContext)
        {
            _resourceManager = resourceManager;
            _globalizationContext = globalizationContext;
        }

        public string GetString(string key)
        {
            return _resourceManager.GetString(key, new CultureInfo(_globalizationContext.PreferedLanguage));
        }

        public string Format(string key, params object[] args)
        {
            return string.Format(GetString(key), args);
        }

        public string Format(string key, object arg0, object arg1, object arg2)
        {
            return string.Format(GetString(key), arg0, arg1, arg2);
        }

        public string Format(string key, object arg0, object arg1)
        {
            return string.Format(GetString(key), arg0, arg1);
        }

        public string Format(string key, object arg0)
        {
            return string.Format(GetString(key), arg0);
        }
    }
}