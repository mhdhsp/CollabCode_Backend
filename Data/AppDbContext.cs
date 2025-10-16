using CollabCode.Common.Model;
using Microsoft.EntityFrameworkCore;

namespace CollabCode.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

       public  DbSet<UserModel> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserModel>()
                .HasKey(u => u.Id);

        }
    }
}
