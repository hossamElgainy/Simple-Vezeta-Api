namespace VezetaAPI.ApiResponse
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; init; }
        public string? Message { get; init; }
        public T? Data { get; init; }


        public ApiResponse(int statusCode, string? message = null, T data = default!)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
            Data = data;
        }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                200 => "Success",
                400 => "A bad request was made.",
                401 => "Unauthorized access.",
                404 => "The requested resource was not found.",
                500 => "An internal server error has occurred.",
                _ => "An error occurred."
            };
        }
    }

}
