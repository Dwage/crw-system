using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
public class MalfunctionFrequencyConfiguration : IEntityTypeConfiguration<MalfunctionFrequency>
{
    public void Configure(EntityTypeBuilder<MalfunctionFrequency> builder)
    {
        builder.ToView("MalfunctionFrequency");
        builder.HasNoKey();
    }
}