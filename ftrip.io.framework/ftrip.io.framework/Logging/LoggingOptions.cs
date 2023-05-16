namespace ftrip.io.framework.Logging
{
    public class LoggingOptions
    {
        public string ApplicationName { get; set; }
        public string ApplicationLabel { get; set; }
        public string ClientIdAttribute { get; set; } = null;

        public bool WriteToConsole { get; set; } = true;
        public string ConsoleOutputTemplate { get; set; } = "[{Timestamp:yyyy-MM-dd HH:mm:ss} {ClientIp} {Level:u3}] - {Message:lj}{NewLine}";

        public bool WriteToGrafanaLoki { get; set; } = true;
        public string GrafanaLokiUrl { get; set; }
    }
}