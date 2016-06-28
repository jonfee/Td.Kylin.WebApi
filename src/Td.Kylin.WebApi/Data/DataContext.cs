
using Microsoft.EntityFrameworkCore;
using Td.Kylin.WebApi.Models;

namespace Td.Kylin.WebApi.Data
{
    public class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            switch (WebApiConfig.Options.SqlType)
            {
                case EnumLibrary.SqlProviderType.SqlServer:
                    options.UseSqlServer(WebApiConfig.Options.SqlConnectionString);
                    break;
                case EnumLibrary.SqlProviderType.NpgSQL:
                    //options.UseNpgsql(WebApiConfig.Options.SqlConnectionString);
                    break;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<System_ModuleAuthorize>().HasKey(p => new { p.ServerID, p.ModuleID });
        }

        public DbSet<System_ModuleAuthorize> ModuleAuthorize { get; set; }
    }
}
