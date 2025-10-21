using CollabCode.CollabCode.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollabCode.CollabCode.Infrastructure.Persistense.Configurations
{
    public class RoomMemberConfiguration:IEntityTypeConfiguration<RoomMember>
    {
        public void Configure(EntityTypeBuilder<RoomMember> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasOne(u => u.User)
                .WithMany(u => u.MemberShips)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(u => u.Room)
                .WithMany(u => u.Members)
                .HasForeignKey(u => u.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
