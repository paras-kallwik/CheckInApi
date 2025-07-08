// Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using CheckInApi.Model;
namespace CheckInApi.Model;
public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

    public DbSet<UserData> Usersdata { get; set; }
}
