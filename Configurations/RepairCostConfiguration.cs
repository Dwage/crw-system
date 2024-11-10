using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RepairCostConfiguration : IEntityTypeConfiguration<RepairCost>
{
    public void Configure(EntityTypeBuilder<RepairCost> builder)
    {
        builder.ToView("RepairCosts"); 
        builder.HasNoKey();
    }
}
