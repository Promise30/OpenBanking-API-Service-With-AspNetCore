using System.Net;

namespace OpenBanking_API_Service
{
    public class APIResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
        public APIResponse(HttpStatusCode statusCode, T data, string errorMessage)
        {
            StatusCode = statusCode;
            Data = data;
            ErrorMessage = errorMessage;
        }
        public static APIResponse<T> Create(HttpStatusCode statusCode, T data, string errorMessage)
        {
            return new APIResponse<T>(statusCode, data, errorMessage);
        }
    }
}
