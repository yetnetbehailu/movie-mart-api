using System;
using System.ComponentModel.DataAnnotations;

namespace movie_mart_api.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Role name is required")]
        public string RoleName { get; set; }

        public List<User> Users { get; set; } = new List<User>();
    }
}

