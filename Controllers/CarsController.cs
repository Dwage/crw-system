using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class CarsController : GenericController<Car>
{
    public CarsController(CarWorkshopContext context) : base(context) { }
}