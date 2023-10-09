using Microsoft.EntityFrameworkCore;
using ShopList.Models;

namespace ShopList;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

}
