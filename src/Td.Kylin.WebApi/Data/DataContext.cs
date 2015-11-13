using Microsoft.Data.Entity;
using Td.Kylin.WebApi.Models;

namespace Td.Kylin.WebApi.Data
{
    public class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(WebApiConfig.Configuration["Data:APIConnectionString"]);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<System_ModuleAuthorize>().HasKey(p => new { p.ServerID, p.ModuleID });
        }

        public DbSet<System_ModuleAuthorize> ModuleAuthorize { get; set; }
    }
}
