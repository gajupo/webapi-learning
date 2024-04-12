using Microsoft.EntityFrameworkCore;
using webapi_learning.Models;

namespace webapi_learning.Data
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Shirt> Shirts { get; set; }

        public ApplicationDbContext(DbContextOptions options): base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // seed data
            modelBuilder.Entity<Shirt>().HasData(
                new Shirt { Id = 1, Model = "Blue", Gender = "male", Size = 15 },
                new Shirt { Id = 2, Model = "Red", Gender = "women", Size = 8 }
            );
        }
    }
}
