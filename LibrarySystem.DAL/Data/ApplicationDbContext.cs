using LibrarySystem.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
         : base(options)
        {
        }
    }
}
