using System.Collections;

namespace BookStore.Api.Errors
{
    public class ValidationErrorRespons:ApiResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public ValidationErrorRespons():base(400)
        {
            this.Errors = new List<string>();
        }
        
    }
}
