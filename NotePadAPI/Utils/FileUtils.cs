using NotePadAPI.Models;

namespace NotePadAPI.Utils
{
    internal class FileUtils
    {
        internal async static Task<List<Note>> GetNotesFromTextFile(IFormFile file, int userId)
        {
            List<Note> list = new List<Note>();
            using (StreamReader reader = new StreamReader(file.OpenReadStream()))
            {
                string content = await reader.ReadToEndAsync();
                string[] notes = content.Split("[T]", StringSplitOptions.RemoveEmptyEntries);
                foreach (string note in notes)
                {
                    if (!string.IsNullOrWhiteSpace(note))
                    {
                        string[] text = note.Split("[D]", StringSplitOptions.RemoveEmptyEntries);
                        if (text.Length != 2)
                        {
                            list.Clear();
                            return list;
                        }
                        text[0] = text[0].Trim();
                        text[1] = text[1].Trim();
                        Note newNote = new Note() 
                        {
                            Title = text[0],
                            Description = text[1],
                            CreatedAt = DateTime.UtcNow,
                            LastUpdatedAt = DateTime.UtcNow,
                            UserId = userId
                        };
                        list.Add(newNote);
                    }
                }
            }
            return list;
        } 
    }
}
