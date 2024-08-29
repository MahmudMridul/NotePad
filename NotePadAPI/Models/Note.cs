using System.ComponentModel.DataAnnotations;

namespace NotePadAPI.Models
{
    public class Note
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        // Relationship with User
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
