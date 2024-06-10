using BreakTimeApp.Helpers;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace BreakTimeApp.Models
{
    public class SelectNotifyDbContext : DbContext
    {
        
        public DbSet<SelectNotifyDb> SelectNotifyItems { get; set; }

        [LogAspect]
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SelectNotifyDbContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "db";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var dbPath = Path.Join(path, Properties.DbResources.selectNotifyDbName);
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}
