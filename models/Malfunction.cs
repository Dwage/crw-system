using System.ComponentModel.DataAnnotations;

public class Malfunction
{
    public int MalfunctionId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Labor cost cannot be negative.")]
    public decimal LaborCost { get; set; }

    public ICollection<CarRepair> CarRepairs { get; set; } = new List<CarRepair>();
    public ICollection<SparePart> SpareParts { get; set; } = new List<SparePart>();
}
