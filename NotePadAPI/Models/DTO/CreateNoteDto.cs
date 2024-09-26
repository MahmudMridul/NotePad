namespace NotePadAPI.Models.DTO
{
    public class CreateNoteDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int UserId { get; set; }
    }
}
