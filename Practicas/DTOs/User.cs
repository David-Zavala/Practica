using System.ComponentModel.DataAnnotations;

namespace Practicas.DTOs
{
    public class User
    {
        public string Email { get; set; } = "";
        public string Name { get; set; } = "";
        public string BirthDate { get; set; } = "";
    }
}
