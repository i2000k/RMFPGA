using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class StandConfiguration : IEntityTypeConfiguration<Stand>
    {
        public void Configure(EntityTypeBuilder<Stand> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.BoardTitle)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.State)
                .IsRequired();

            builder.Property(s => s.ConnectionUrl)
                .IsRequired()
                .HasMaxLength(500);

            //builder.HasOne(s => s.CurrentSession).WithOne(s => s.Stand);

            builder
                .HasMany(s => s.Sessions)
                .WithOne(s => s.Stand)
                .HasForeignKey(s => s.StandId);
        }
    }
}
