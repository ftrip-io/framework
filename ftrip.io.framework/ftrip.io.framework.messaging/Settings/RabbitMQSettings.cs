namespace ftrip.io.framework.messaging.Settings
{
    public class RabbitMQSettings
    {
        public virtual string Server { get; set; }
        public virtual string Port { get; set; }
        public virtual string User { get; set; }
        public virtual string Password { get; set; }

        public string GetConnectionString()
        {
            return $"rabbitmq://{Server}:{Port}";
        }
    }
}