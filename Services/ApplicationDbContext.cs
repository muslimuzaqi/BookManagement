using BookMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace BookMVC.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<Book> Books { get; set; }   
    }
}
