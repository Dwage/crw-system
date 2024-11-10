using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.HasKey(t => t.TeamId);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(t => t.StaffMembers)
            .WithOne(s => s.Team)
            .HasForeignKey(s => s.TeamId)
            .OnDelete(DeleteBehavior.Cascade); 

        builder.HasMany(t => t.CarRepairs)
            .WithOne(cr => cr.Team)
            .HasForeignKey(cr => cr.TeamId)
            .OnDelete(DeleteBehavior.Restrict); 
    }
}
