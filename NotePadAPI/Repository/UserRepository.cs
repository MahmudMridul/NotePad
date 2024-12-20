﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _db.Users.AsNoTracking().ToListAsync();
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _db.Users.AnyAsync(user => user.Email == email);   
        }

        public async Task<User> RegisterUser(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(user => user.Email == email);
        }
    }
}
