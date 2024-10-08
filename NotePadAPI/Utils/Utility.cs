﻿using NotePadAPI.Models;
using System.Net;

namespace NotePadAPI.Utils
{
    public class Utility
    {
        internal static ApiResponse CreateResponse(string msg, HttpStatusCode code, object? data = null, bool isSuccess = false)
        {
            return new ApiResponse
            {
                Message = msg,
                StatusCode = code,
                Data = data,
                IsSuccess = isSuccess
            };
        }

        internal static string ResponseToString(ApiResponse res)
        {
            return $"Status: {(int)res.StatusCode} {res.Message} Data: {res.Data}";
        }
    }
}
