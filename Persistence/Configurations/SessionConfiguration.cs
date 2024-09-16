using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.State)
                .IsRequired();

            //builder.HasOne(s => s.User)
            //    .WithOne(u => u.CurrentSession);

            //builder.HasOne(s => s.Stand)
            //    .WithOne(s => s.CurrentSession);
        }
    }
}
