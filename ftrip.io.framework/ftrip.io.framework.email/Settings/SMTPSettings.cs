namespace ftrip.io.framework.email.Settings
{
    public class SMTPSettings
    {
        public virtual string Host { get; set; }
        public virtual int Port { get; set; }
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
    }
}