namespace DatingApp.Api.Errors
{
    public class ApiException
    {
        public ApiException(
            int statusCode,
            string message = null,
            string details = null)
        {
            this.Message = message;
            this.Details = details;
            this.StatusCode = statusCode;
        }

        public string Details { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }
}