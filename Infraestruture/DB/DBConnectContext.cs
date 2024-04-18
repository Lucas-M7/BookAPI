using BookAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Infraestruture.DB;
public class DBConnectContext(DbContextOptions<DBConnectContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Book> Books { get; set; } = default!;
}