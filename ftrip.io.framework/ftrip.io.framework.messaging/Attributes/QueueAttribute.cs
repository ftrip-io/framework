using System;

namespace ftrip.io.framework.messaging.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class QueueAttribute : Attribute
    {
        public string Name { get; set; }
    }
}