using MagicVilla_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Villa> Villas { get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Villa>().HasData(
            new Villa(){
                Id = 1,
                Name = "Royal Villa",
                Details = "Some long detail desc",
                ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS8FmUXVqZHmza8ov8uBcWOc3E4agtUuCOjBKzFPcQjwA&s",
                Occupancy = 5,
                Rate = 200,
                Sqft = 550,
                Amenity = "elevators (lifts), internet access, restaurants, parks, community centres, swimming pools, golf courses, health club facilities, party rooms, theater or media rooms, bike paths or garages.",
                CreatedDate = DateTime.Now
            }
            );
    }
}