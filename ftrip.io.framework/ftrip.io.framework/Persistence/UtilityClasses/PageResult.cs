using System.Collections.Generic;

namespace ftrip.io.framework.Persistence.UtilityClasses
{
    public class PageResult<T>
    {
        public IEnumerable<T> Entities { get; set; }
        public int TotalPages { get; set; }
        public int TotalEntities { get; set; }
    }
}