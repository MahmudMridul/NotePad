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

        public async Task<Note?> GetNoteForUser(int noteId)
        {
            Note? n = await (from note in _db.Notes
                            where note.Id == noteId
                            select note).FirstOrDefaultAsync();

            //Note? note = await _db.Notes.FirstOrDefaultAsync(note => note.Id == noteId);
            return n;
        }
    }
}
