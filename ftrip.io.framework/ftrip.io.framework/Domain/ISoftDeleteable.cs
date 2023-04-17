namespace ftrip.io.framework.Domain
{
    public interface ISoftDeleteable
    {
        public bool Active { get; set; }
    }
}