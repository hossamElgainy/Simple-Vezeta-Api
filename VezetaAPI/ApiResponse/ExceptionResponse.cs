using VezetaAPI.ApiResponse;

namespace VezetaApi.ApiResponse
{
    public class ExceptionResponse : ApiResponse<string>
    {
        public string? Details { get; set; }

        public ExceptionResponse(int statusCode, string? message = null, string? details = null) : base(statusCode, message)
        {

            Details = details;
        }

    }
}
