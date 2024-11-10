using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class CarRepairsController : GenericController<CarRepair>
{
    public CarRepairsController(CarWorkshopContext context) : base(context) { }
}