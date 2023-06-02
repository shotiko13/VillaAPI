using Microsoft.EntityFrameworkCore;
using VillaAPI.Models;

namespace VillaAPI.Data
{
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Name = "Kakheti Villa",
                    Description = "LoremIpsum",
                    ImageUrl = "",
                    Occupancy = 5,
                    Rate = 5,
                    Sqft = 5,
                    Amenity = ""
                }
                );
            ;
        }

    }
}
