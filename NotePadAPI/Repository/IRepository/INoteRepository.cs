﻿using NotePadAPI.Models;

namespace NotePadAPI.Repository.IRepository
{
    public interface INoteRepository
    {
        Task<IEnumerable<Note>> GetNotesForUser(string email);
        Task<Note?> GetNoteById(int noteId);
        Task<Note> CreateNote(Note note);
        Task<Note> UpdateNote(Note note);
        Task<Note> DeleteNote(Note note);
    }
}
