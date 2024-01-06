using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OpenBanking_API_Service
{
    public class APIResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public T Data { get; set; }
        public Error Error { get; set; }
        public APIResponse(HttpStatusCode statusCode, string message, T data, Error error = null)
        {
            StatusCode = statusCode;
            StatusMessage = message;
            Data = data;
            Error = error;
        }
        public static APIResponse<T> Create(HttpStatusCode statusCode, string message, T data, Error error = null)
        {
            return new APIResponse<T>(statusCode, message, data, error);
        }
    }
}
