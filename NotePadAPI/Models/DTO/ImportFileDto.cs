namespace NotePadAPI.Models.DTO
{
    public class ImportFileDto
    {
        public IFormFile File { get; set; } = null!;
        public int Id { get; set; }
    }
}
