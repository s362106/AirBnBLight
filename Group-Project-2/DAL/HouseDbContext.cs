using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Group_Project_2.Models;

namespace Group_Project_2.DAL;
public class HouseDbContext : IdentityDbContext<User>
{
    public HouseDbContext(DbContextOptions<HouseDbContext> options) : base(options)
    {
    }

    public DbSet<House> Houses { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }
}
