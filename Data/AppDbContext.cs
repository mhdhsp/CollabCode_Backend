using CollabCode.Common.Model;
using Microsoft.EntityFrameworkCore;

namespace CollabCode.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

       public  DbSet<UserModel> Users { get; set; }
        public DbSet<RoomModel >Rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserModel>()
                .HasKey(u => u.Id);
            modelBuilder.Entity<RoomModel>()
                .HasOne(u => u.Owner)
                .WithMany(s => s.Rooms)
                .HasForeignKey(p => p.OwnerId);

        }
    }
}
