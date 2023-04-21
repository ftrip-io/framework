namespace ftrip.io.framework.Contexts
{
    public class GlobalizationContext
    {
        public string PreferedLanguage { get; set; }

        public GlobalizationContext()
        {
            PreferedLanguage = "jp";
        }
    }
}