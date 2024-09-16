using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(s => s.Email).IsRequired();
            builder.Property(s => s.PasswordHash).IsRequired();
            builder.Property(s => s.PasswordSalt).IsRequired();
            // builder.Property(s => s.Password).IsRequired();
            builder
                .HasMany(s => s.Sessions)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId);
            //builder.HasOne(s => s.CurrentSession).WithOne(s => s.User);
        }
    }
}
