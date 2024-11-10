using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class SparePartsController : GenericController<SparePart>
{
    public SparePartsController(CarWorkshopContext context) : base(context) { }
}