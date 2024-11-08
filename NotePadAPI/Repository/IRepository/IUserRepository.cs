using NotePadAPI.Models;

namespace NotePadAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<bool> EmailExists(string email);
        Task<User> RegisterUser(User user);
        Task<User?> GetUserByEmail(string email);
    }
}
