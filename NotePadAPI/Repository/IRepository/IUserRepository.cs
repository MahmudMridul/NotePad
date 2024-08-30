using Microsoft.AspNetCore.Mvc;
using NotePadAPI.Models;

namespace NotePadAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<bool> EmailExists(string email);
        Task<ActionResult<ApiResponse>> RegisterUser(User user);
    }
}
