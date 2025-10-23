using CollabCode.CollabCode.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CollabCode.CollabCode.Infrastructure.Persistense
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

       public  DbSet<User> Users { get; set; }
        public DbSet<Project>Projects { get; set; }
        public DbSet<ProjectFile> ProjectFiles { get; set; }
        public DbSet<MemberShip> MemberShips { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
