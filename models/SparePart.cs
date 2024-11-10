using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class SparePart
{
    public int PartId { get; set; }

    [Required]
    public int ModelId { get; set; }

    [Required]
    public int MalfunctionId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Price cannot be negative.")]
    public decimal Price { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
    public int Quantity { get; set; }

    [ForeignKey("ModelId")]
    public virtual CarModel? CarModel { get; set; }

    [ForeignKey("MalfunctionId")]
    public virtual Malfunction? Malfunction { get; set; }
}