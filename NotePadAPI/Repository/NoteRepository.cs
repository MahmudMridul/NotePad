using Microsoft.EntityFrameworkCore;
using NotePadAPI.Db.IDb;
using NotePadAPI.Models;
using NotePadAPI.Repository.IRepository;

namespace NotePadAPI.Repository
{
    public class NoteRepository : INoteRepository
    {
        private readonly IDbContext _db;

        public NoteRepository(IDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Note>> GetNotesForUser(string email)
        {
            return await (from user in _db.Users
                          where user.Email == email
                          from note in user.Notes
                          select note).ToListAsync();

            //return await _db.Users
            //    .Where(user => user.Email == email)
            //    .SelectMany(user => user.Notes)
            //    .ToListAsync();
        }
    }
}
