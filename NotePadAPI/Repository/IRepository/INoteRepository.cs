using NotePadAPI.Models;

namespace NotePadAPI.Repository.IRepository
{
    public interface INoteRepository
    {
        Task<IEnumerable<Note>> GetNotesForUser(string email);
        Task<Note?> GetNoteForUser(int noteId);
    }
}
