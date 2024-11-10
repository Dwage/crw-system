using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class MalfunctionsController : GenericController<Malfunction>
{
    public MalfunctionsController(CarWorkshopContext context) : base(context) { }
}
