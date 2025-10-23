using CollabCode.CollabCode.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollabCode.CollabCode.Infrastructure.Persistense.Configurations
{
    public class FileConfiguration:IEntityTypeConfiguration<ProjectFile>
    {
        public void Configure(EntityTypeBuilder<ProjectFile> builder)
        {
            builder.HasKey(u => u.Id);
            builder.HasOne(u => u.Project)
                .WithMany(u => u.Files)
                .HasForeignKey(u => u.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
