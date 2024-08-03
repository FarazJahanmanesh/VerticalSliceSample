using Microsoft.EntityFrameworkCore;
using Sample.Api.Entities;
namespace Sample.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Book> Books { get; set; }
}
