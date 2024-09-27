using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET___CRUD.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? City { get; set; }
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public ApplicationUser()
        {
            
        }
    }
}
