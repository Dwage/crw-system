using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Team
{
    public int TeamId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    public ICollection<Staff> StaffMembers { get; set; } = new List<Staff>();
    public ICollection<CarRepair> CarRepairs { get; set; } = new List<CarRepair>();
}