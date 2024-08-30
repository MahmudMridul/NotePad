using NotePadAPI.Repository.IRepository;

namespace NotePadAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        public Task<bool> EmailExists(string email)
        {
            
        }
    }
}
