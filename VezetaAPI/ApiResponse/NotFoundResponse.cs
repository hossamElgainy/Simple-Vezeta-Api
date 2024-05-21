namespace VezetaApi.ApiResponse
{
    public class NotFoundResponse
    {
        public int StatusCode { get; init; }
        public string? Message { get; init; }

        public NotFoundResponse(string? message)
        {
            StatusCode = 404;
            Message = message ?? "The requested resource was not found.";
        }
    }
}
