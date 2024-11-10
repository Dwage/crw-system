using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class StaffController : GenericController<Staff>
{
    public StaffController(CarWorkshopContext context) : base(context) { }
}