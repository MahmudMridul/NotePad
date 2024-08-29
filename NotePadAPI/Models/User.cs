using System.ComponentModel.DataAnnotations;

namespace NotePadAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        [MaxLength(100)]
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        public ICollection<Note> Notes { get; set; } = new List<Note>(1);
    }
}
