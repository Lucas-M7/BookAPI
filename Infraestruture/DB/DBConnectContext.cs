using BookAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Infraestruture.DB;
public class DBConnectContext(DbContextOptions<DBConnectContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Email = "Lucas@teste.com",
                Password = "pass123",
                Profile = "ADM"
            }
        );
    }

    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Book> Books { get; set; } = default!;
}