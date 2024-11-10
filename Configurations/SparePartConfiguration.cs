using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SparePartConfiguration : IEntityTypeConfiguration<SparePart>
{
    public void Configure(EntityTypeBuilder<SparePart> builder)
    {
        builder.HasKey(sp => sp.PartId);

        builder.Property(sp => sp.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(sp => sp.Price)
            .HasPrecision(10, 2)
            .HasColumnType("decimal(10,2)");

        builder.Property(sp => sp.Quantity)
            .IsRequired();

        builder.HasOne(sp => sp.CarModel)
            .WithMany(cm => cm.SpareParts)
            .HasForeignKey(sp => sp.ModelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(sp => sp.Malfunction)
            .WithMany(m => m.SpareParts)
            .HasForeignKey(sp => sp.MalfunctionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}