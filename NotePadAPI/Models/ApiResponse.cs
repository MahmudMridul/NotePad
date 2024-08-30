using System.Net;

namespace NotePadAPI.Models
{
    public class ApiResponse
    {
        public object? Data { get; set; } = null;
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = false;
        public string? Message { get; set; } = string.Empty;
    }
}
