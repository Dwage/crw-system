using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CarRepairConfiguration : IEntityTypeConfiguration<CarRepair>
{
    public void Configure(EntityTypeBuilder<CarRepair> builder)
    {
        builder.HasKey(cr => cr.RepairId);

        builder.Property(cr => cr.StartDate)
            .IsRequired();

        builder.Property(cr => cr.EndDate)
            .IsRequired(false);
    }
}
