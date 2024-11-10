using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Workshop
{
    public int WorkshopId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    public string Address { get; set; }

    public ICollection<Staff> StaffMembers { get; set; } = new List<Staff>();
    
}
