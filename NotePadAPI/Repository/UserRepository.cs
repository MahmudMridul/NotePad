using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotePadAPI.Db.IDb;
using NotePadAPI.Models;
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

        public async void RegisterUser(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }
    }
}
