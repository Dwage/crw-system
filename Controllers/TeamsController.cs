using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")] 

public class TeamsController : GenericController<Team>
{
    public TeamsController(CarWorkshopContext context) : base(context) { }
}
