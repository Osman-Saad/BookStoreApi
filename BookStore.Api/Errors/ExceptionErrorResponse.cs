namespace BookStore.Api.Errors
{
    public class ExceptionErrorResponse:ApiResponse
    {
        public string? Details { get; set; }
        public ExceptionErrorResponse(int StatusCode,string?Message=null,string?Details=null):base(StatusCode,Message)
        {
            this.Details = Details;
        }
    }
}
