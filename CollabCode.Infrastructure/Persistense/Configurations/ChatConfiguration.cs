using CollabCode.CollabCode.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollabCode.CollabCode.Infrastructure.Persistense.Configurations
{
    public class ChatConfiguration:IEntityTypeConfiguration<Chat> 
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.HasKey(u => u.Id);
            builder.HasOne(u => u.Project)
                .WithMany()
                .HasForeignKey(u=>u.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(u => u.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
