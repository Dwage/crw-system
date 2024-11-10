using Microsoft.EntityFrameworkCore;

public class CarWorkshopContext : DbContext
{
    public CarWorkshopContext(DbContextOptions<CarWorkshopContext> options)
        : base(options) { }

    public DbSet<Workshop> Workshops { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Staff> Staff { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<Malfunction> Malfunctions { get; set; }
    public DbSet<CarRepair> CarRepairs { get; set; }
    public DbSet<SparePart> SpareParts { get; set; }
    public DbSet<CarModel> CarModels { get; set; }

    public DbSet<RepairCost> RepairCosts { get; set; }
    public DbSet<LaborPerformance> LaborPerformance { get; set; }
    public DbSet<MalfunctionFrequency> MalfunctionFrequency { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new WorkshopConfiguration());
        modelBuilder.ApplyConfiguration(new TeamConfiguration());
        modelBuilder.ApplyConfiguration(new StaffConfiguration());
        modelBuilder.ApplyConfiguration(new CarConfiguration());
        modelBuilder.ApplyConfiguration(new MalfunctionConfiguration());
        modelBuilder.ApplyConfiguration(new CarRepairConfiguration());
        modelBuilder.ApplyConfiguration(new SparePartConfiguration());
        modelBuilder.ApplyConfiguration(new CarModelConfiguration());
        modelBuilder.ApplyConfiguration(new RepairCostConfiguration());
        modelBuilder.ApplyConfiguration(new LaborPerformanceConfiguration());
        modelBuilder.ApplyConfiguration(new MalfunctionFrequencyConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
