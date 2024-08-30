using Microsoft.EntityFrameworkCore;
using NotePadAPI.Db.IDb;
using NotePadAPI.Repository.IRepository;

namespace NotePadAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbContext _db;

        public UserRepository(IDbContext db)
        {
            _db = db;
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _db.Users.AnyAsync(user => user.Email == email);   
        }
    }
}
