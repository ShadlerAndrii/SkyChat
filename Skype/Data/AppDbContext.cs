using Microsoft.EntityFrameworkCore;

namespace Skype.Data
{
    public class AppDbContext : DbContext
    {
        public IConfiguration _configuration {  get; set; }
        public AppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DatabaseConnection"));
        }

        public DbSet<User> Users { get; set; }        
        public DbSet<Setting> Setting { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
