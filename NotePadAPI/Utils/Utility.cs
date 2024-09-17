using NotePadAPI.Models;
using System.Net;

namespace NotePadAPI.Utils
{
    public class Utility
    {
        internal static ApiResponse CreateResponse(string msg, HttpStatusCode code, object? data = null, bool isSuccess = false)
        {
            ApiResponse response = new ApiResponse();
            response.Message = msg;
            response.StatusCode = code;
            response.Data = data;
            response.IsSuccess = isSuccess;
            return response;
        }
    }
}
