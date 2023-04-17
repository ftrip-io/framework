using System;

namespace ftrip.io.framework.Mapping
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MappableAttribute : Attribute
    {
        public Type Destination { get; set; }
    }
}