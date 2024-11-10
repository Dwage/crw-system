using System.ComponentModel.DataAnnotations;

public class Car
{
    public int CarId { get; set; }

    [Required]
    public int ModelId { get; set; }

    [Required]
    [StringLength(50)]
    public string BodyNumber { get; set; }

    [Required]
    [StringLength(50)]
    public string EngineNumber { get; set; }

    [Required]
    [StringLength(100)]
    public string Owner { get; set; }

    [Required]
    [StringLength(50)]
    public string FactoryNumber { get; set; }

    public ICollection<CarRepair> CarRepairs { get; set; } = new List<CarRepair>();
    public virtual CarModel CarModel { get; set; }
}
