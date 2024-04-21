using Microsoft.EntityFrameworkCore;

namespace MVCMessenger.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
    }
}
