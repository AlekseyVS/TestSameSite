using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace AuthApp.Models
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public UserContext(DbContextOptions<UserContext> options) : base(options) {}
    }
}