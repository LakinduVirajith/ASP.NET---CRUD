using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ASP.NET___CRUD.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASP.NET___CRUD.Data
{
    // Add-Migration InitialCreate -OutputDir Data/Migrations
    // Remove-Migration
    // Update-Database
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ASP.NET___CRUD.Models.Joke> Joke { get; set; } = default!;
        public DbSet<ASP.NET___CRUD.Models.ApplicationUser> ApplicationUser { get; set; } = default!;
    }
}
