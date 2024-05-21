namespace VezetaApi.ApiResponse
{
    public class BadRequestResponse
    {
        public int StatusCode { get; init; }
        public string? Message { get; init; }

        public BadRequestResponse(string? message)
        {
            StatusCode = 400;
            Message = message ?? "A bad request was made.";
        }
    }
}
