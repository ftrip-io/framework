using System.Collections.Generic;

namespace ftrip.io.framework.Validation
{
    public class ValidationErrorResponse
    {
        public List<ValidationErrorModel> Errors { get; set; } = new List<ValidationErrorModel>();
    }
}