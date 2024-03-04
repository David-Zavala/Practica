using System.ComponentModel.DataAnnotations;

namespace Practicas.DTOs
{
    public class User
    {
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
        public string? BirthDate { get; set; }
    }
}
