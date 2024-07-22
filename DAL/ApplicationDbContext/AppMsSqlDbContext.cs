using DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.ApplicationDbContext
{
    public class AppMsSqlDbContext : IdentityDbContext<User>
    {
        public AppMsSqlDbContext(DbContextOptions<AppMsSqlDbContext> options)
            : base(options) { }

        public DbSet<TestModel> TestModels { get; set; }
        public DbSet<Partners> Partner { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
