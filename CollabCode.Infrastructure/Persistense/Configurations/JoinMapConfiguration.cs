using CollabCode.CollabCode.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollabCode.CollabCode.Infrastructure.Persistense.Configurations
{
    public class JoinMapConfiguration:IEntityTypeConfiguration<JoinMap>
    {
        public void Configure(EntityTypeBuilder<JoinMap> builder)
        {
            builder.HasKey(u => u.Id);
            builder.HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(u => u.userId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
