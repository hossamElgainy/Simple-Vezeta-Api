using VezetaAPI.ApiResponse;

namespace VezetaApi.ApiResponse
{
    public class ValidationErrorResponse : ApiResponse<IEnumerable<string>>
    {
        public IEnumerable<string>? Errors { get; init; }
        public ValidationErrorResponse(IEnumerable<string> errors)
            : base(400, "Validation failures have occurred.", errors)
        {
            Errors = errors ?? new List<string>();

        }
    }
}
