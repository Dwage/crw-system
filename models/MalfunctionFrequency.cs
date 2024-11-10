using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class MalfunctionFrequency
{
    public string MalfunctionName { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public decimal TotalRepairs { get; set; }
}