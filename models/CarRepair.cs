using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CarRepair
{
    public int RepairId { get; set; }

    [ForeignKey("Car")]
    public int CarId { get; set; }

    [ForeignKey("Malfunction")]
    public int MalfunctionId { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [ForeignKey("Team")]
    public int TeamId { get; set; }

    public virtual Car? Car { get; set; }
    public virtual Malfunction? Malfunction { get; set; }
    public virtual Team? Team { get; set; }

    public bool IsValid()
    {
        return EndDate == null || EndDate >= StartDate;
    }
}
