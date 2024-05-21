namespace VezetaApi.ApiResponse
{
    public class OkResponse<T>
    {
        public int StatusCode { get; init; }
        public string? Message { get; init; }
        public T? Data { get; init; }

        public OkResponse(T data = default!, string? message = null)
        {
            StatusCode = 200;
            Message = message ?? "data Retreived Successfully";
            Data = data;
        }

    }

}
