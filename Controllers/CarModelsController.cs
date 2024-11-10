using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class CarModelsController : GenericController<CarModel>
{
    public CarModelsController(CarWorkshopContext context) : base(context) { }
}