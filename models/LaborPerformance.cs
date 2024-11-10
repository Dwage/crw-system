using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class LaborPerformance
{
    public int TeamId { get; set; }
    public string TeamName { get; set; }
    public long RepairsCount { get; set; }
    public decimal TotalDaysSpent { get; set; }
    public decimal AvgDaysPerOrder { get; set; }
}