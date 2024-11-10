using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class RepairCost
{
    public string Owner { get; set; }
    public int CarId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal TotalPrice { get; set; }
    public int[] MalfunctionIds { get; set; }
}