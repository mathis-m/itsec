using System.Reflection;
using HackMeApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace HackMeApi.Infrastructure
{
    public class HackMeContext: DbContext
    {
        public readonly bool IsSqlLite;
        public DbSet<Post> Posts { get; set; }
        public DbSet<AppUser> AppUser { get; set; }

        public string ConnectionString { get; }

        public HackMeContext(IConfiguration configuration)
        {
            var connStr = configuration.GetValue<string>("SqlConnection");
            if (connStr == null)
            {
                const Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
                var path = Environment.GetFolderPath(folder);
                ConnectionString = $"Data Source={Path.Join(path, "hackMe.db")}";
                IsSqlLite = true;
            }
            else
            {
                IsSqlLite = false;
                ConnectionString = connStr;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (IsSqlLite)
                options.UseSqlite(ConnectionString);
            else
                options.UseNpgsql(ConnectionString);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if(IsSqlLite)
                modelBuilder.UseSerialColumns();
            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.UserName)
                .IsUnique();
            base.OnModelCreating(modelBuilder);
        }
    }
}
