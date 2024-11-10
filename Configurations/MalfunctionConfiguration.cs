using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class MalfunctionConfiguration : IEntityTypeConfiguration<Malfunction>
{
    public void Configure(EntityTypeBuilder<Malfunction> builder)
    {
        builder.HasKey(m => m.MalfunctionId);

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.LaborCost)
            .HasPrecision(10, 2) 
            .HasColumnType("decimal(10,2)");

        builder.HasMany(m => m.CarRepairs)
            .WithOne(cr => cr.Malfunction)
            .HasForeignKey(cr => cr.MalfunctionId);

        builder.HasMany(m => m.SpareParts)
            .WithOne(sp => sp.Malfunction)
            .HasForeignKey(sp => sp.MalfunctionId);
    }
}
