using CollabCode.CollabCode.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollabCode.CollabCode.Infrastructure.Persistense.Configurations
{
    public class VersionConfiguration:IEntityTypeConfiguration<FileVersion>
    {
        public void Configure(EntityTypeBuilder<FileVersion> builder)
        {
            builder.HasKey(u => u.Id);
            builder.HasOne(u => u.File)
                .WithMany()
                .HasForeignKey(u => u.FileId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
