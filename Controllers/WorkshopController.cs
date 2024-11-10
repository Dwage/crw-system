using Microsoft.AspNetCore.Mvc;
[Route("api/workshops")]
public class WorkshopsController : GenericController<Workshop>
{
    public WorkshopsController(CarWorkshopContext context) : base(context) { }
}