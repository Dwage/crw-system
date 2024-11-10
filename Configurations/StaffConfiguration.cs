using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class StaffConfiguration : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        builder.HasKey(s => s.PersonInn);

        builder.Property(s => s.FullName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Position)
            .HasMaxLength(50);

        builder.Property(s => s.Salary)
            .HasPrecision(10, 2) 
            .HasColumnType("decimal(10,2)");

        builder.Property(s => s.HireDate)
            .IsRequired();

    }
}
