using System.ComponentModel.DataAnnotations;

namespace AssetAdminUI.Models
{
    public class CreateUser
    {
        [Required]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string ContactNumber { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Role { get; set; } // "Admin" or "Employee"
    }
}
