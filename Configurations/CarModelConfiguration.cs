using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CarModelConfiguration : IEntityTypeConfiguration<CarModel>
{
    public void Configure(EntityTypeBuilder<CarModel> builder)
    {
        builder.HasKey(cm => cm.ModelId);

        builder.Property(cm => cm.Brand)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(cm => cm.Model)
            .IsRequired()
            .HasMaxLength(100);

         builder.HasMany(cm => cm.Cars)
            .WithOne(c => c.CarModel)
            .HasForeignKey(c => c.ModelId);

        builder.HasMany(cm => cm.SpareParts)
            .WithOne(sp => sp.CarModel)
            .HasForeignKey(sp => sp.ModelId);
    }
}
