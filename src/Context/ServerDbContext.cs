using fridgeplus_server.Models;
using Microsoft.EntityFrameworkCore;

namespace fridgeplus_server.Context
{
    public class ServerDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseMySQL(Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING"));
    }
}
