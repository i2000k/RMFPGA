using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.Property(s => s.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(s => s.SecondName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(s => s.Grade)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(s => s.GradeYear)
                .IsRequired();
            builder.Property(s => s.Group)
                .IsRequired();



        }
    }
}
