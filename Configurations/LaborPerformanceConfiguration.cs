using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
public class LaborPerformanceConfiguration : IEntityTypeConfiguration<LaborPerformance>
{
    public void Configure(EntityTypeBuilder<LaborPerformance> builder)
    {
        builder.ToView("LaborPerformance");
        builder.HasNoKey();
    }
}