using Microsoft.EntityFrameworkCore;
using ProfileService.Models;

namespace ProfileService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<ProfileInfra> Profiles { get; set; }
}