using Microsoft.EntityFrameworkCore;
using Group_Project_2.Models;

namespace Group_Project_2.DAL;
public class HouseDbContext : DbContext
{
	public HouseDbContext(DbContextOptions<HouseDbContext> options) : base(options)
	{
	}

	public DbSet<House> Houses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
		optionsBuilder.UseLazyLoadingProxies();
    }
}
