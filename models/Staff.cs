using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Staff
{
    [Key]
    [StringLength(12)]
    public string PersonInn { get; set; }

    [ForeignKey("Workshop")]
    public int WorkshopId { get; set; }

    [ForeignKey("Team")]
    public int TeamId { get; set; }

    [Required]
    [StringLength(100)]
    public string FullName { get; set; }

    [StringLength(50)]
    public string Position { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Salary must be greater than 0.")]
    public decimal Salary { get; set; }

    [Required]
    public DateTime HireDate { get; set; } = DateTime.Now;

    public virtual Workshop? Workshop { get; set; }
    public virtual Team? Team { get; set; }
}
