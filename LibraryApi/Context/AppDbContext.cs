using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Context
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}
