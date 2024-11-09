using fridgeplus_server.Models;
using Microsoft.EntityFrameworkCore;

namespace fridgeplus_server.Context
{
    public class ServerDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            string? connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");

            if (connectionString is null) throw new ArgumentException("Environment variable MYSQL_CONNECTION_STRING was null.");
            options.UseMySQL(connectionString);
        }
    }
}
