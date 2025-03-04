using EANValidator.Models;
using Microsoft.EntityFrameworkCore;
namespace EANValidator.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Nitem> Nitems{ get; set; }
    }
}