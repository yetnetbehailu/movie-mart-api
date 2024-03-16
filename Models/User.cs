using System;
using System.ComponentModel.DataAnnotations;

namespace movie_mart_api.Models
{
	public class User
	{
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Token { get; set; }

        // Many-to-many relation with roles
        public List<Role> Roles { get; set; } = new List<Role>();
    }
}

