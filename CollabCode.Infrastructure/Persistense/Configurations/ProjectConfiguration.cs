using CollabCode.CollabCode.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollabCode.CollabCode.Infrastructure.Persistense.Configurations
{
    public class ProjectConfiguration:IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasOne(u => u.Owner)
                .WithMany(u => u.Projects)
                .HasForeignKey(u => u.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
