namespace BookStore.Api.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ApiResponse(int StatusCode,string? Message=null)
        {
            this.StatusCode = StatusCode;
            this.Message = string.IsNullOrEmpty(Message) ? GetErrorMessage(StatusCode) : Message;
        }

        private string GetErrorMessage(int StatusCode) => StatusCode switch
        {
            400 => "Bad Request",
            401 => "Unauthorized",
            404 => "Not Found",
            500 => "Internal Server Error",
            _ => "Something went wrong"
        };
    }
}
