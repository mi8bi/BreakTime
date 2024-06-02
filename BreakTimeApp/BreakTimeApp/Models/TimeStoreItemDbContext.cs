using BreakTimeApp.Helpers;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace BreakTimeApp.Models
{
    public class TimeStoreItemDbContext : DbContext
    {

        public DbSet<TimeStoreItemDb> TimeStoreItems { get; set; }

        [LogAspect]
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TimeStoreItemDbContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "db";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var dbPath = Path.Join(path, Properties.DbResources.timeStoreItemsDbName);
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}
