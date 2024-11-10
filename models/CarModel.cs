using System.ComponentModel.DataAnnotations;

public class CarModel
{
    public int ModelId { get; set; }

    [Required]
    [StringLength(50)]
    public string Brand { get; set; }

    [Required]
    [StringLength(100)]
    public string Model { get; set; }


    public ICollection<Car> Cars { get; set; } = new List<Car>();
    public ICollection<SparePart> SpareParts { get; set; } = new List<SparePart>();
}
