using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.HasKey(c => c.CarId);
        
        builder.Property(c => c.ModelId)
            .IsRequired();

        builder.Property(c => c.BodyNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.EngineNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Owner)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.FactoryNumber)
            .IsRequired()
            .HasMaxLength(50);
    }
}
