using Prometheus;

namespace ftrip.io.framework.Metrics
{
    public class DataTrafficCounter
    {
        private readonly Counter _dataTrafficCounter;

        public DataTrafficCounter()
        {
            _dataTrafficCounter = Prometheus.Metrics.CreateCounter(
                "data_traffic_bytes",
                "The total data traffic in bytes."
            );
        }

        public void RegisterTraffic(long dataLen)
        {
            _dataTrafficCounter.Inc(dataLen);
        }
    }

    public class VisitorCounter
    {
        private readonly Counter _visitorCounter;

        public VisitorCounter()
        {
            _visitorCounter = Prometheus.Metrics.CreateCounter(
                "total_visitors",
                "The total number of visitors.",
                new CounterConfiguration { LabelNames = new[] { "ip_address", "web_browser" } }
            );
        }

        public void RegisterRequest(string ipAddress, string webBrowser)
        {
            _visitorCounter.WithLabels(ipAddress, webBrowser).Inc();
        }
    }
}
