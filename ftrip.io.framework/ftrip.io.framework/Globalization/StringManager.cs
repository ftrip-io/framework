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
    }
}