using System.ComponentModel.DataAnnotations;

namespace NotePadAPI.Models.DTO
{
    public class UserRegistrationDto
    {
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        [MaxLength(100)]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
