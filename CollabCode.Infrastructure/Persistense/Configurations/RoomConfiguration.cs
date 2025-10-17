using CollabCode.CollabCode.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollabCode.CollabCode.Infrastructure.Persistense.Configurations
{
    public class RoomConfiguration:IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasOne(u => u.Owner)
                .WithMany(u => u.Rooms)
                .HasForeignKey(u => u.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
