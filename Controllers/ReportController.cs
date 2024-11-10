using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


[ApiController]
[Route("api/reports")]
public class ReportController : ControllerBase
{
    private readonly CarWorkshopContext _context;

    public ReportController(CarWorkshopContext context)
    {
        _context = context;
    }

    [HttpGet("repair-costs")]
    public async Task<IActionResult> GetRepairCosts(
        [FromQuery] string owner = null,
        [FromQuery] int? carId = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] string sortColumn = "totalPrice",
        [FromQuery] string sortDirection = "asc")
    {
        try
        {
            var query = _context.RepairCosts.AsQueryable();

            if (!string.IsNullOrEmpty(owner))
            {
                query = query.Where(r => r.Owner.Contains(owner));
            }

            if (carId.HasValue)
            {
                query = query.Where(r => r.CarId == carId.Value);
            }

            if (startDate.HasValue)
            {
                query = query.Where(r => r.StartDate >= DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc));
            }

            if (endDate.HasValue)
            {
                query = query.Where(r => r.EndDate <= DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc));
            }

            query = ApplySorting(query, sortColumn, sortDirection);


            var results = await query.ToListAsync();
            var totalSum = results.Sum(r => r.TotalPrice);

            return Ok(new { items = results, totalSum = totalSum });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while generating the report.", error = ex.Message });
        }
    }



    [HttpGet("labor-performance")]
    public async Task<IActionResult> GetLaborPerformance(
        [FromQuery] string sortColumn = "RepairsCount",
        [FromQuery] string sortDirection = "asc")
    {
        try
        {
            var query = _context.LaborPerformance.AsQueryable();

            query = ApplySorting(query, sortColumn, sortDirection);

            var results = await query.ToListAsync();
            return Ok(new { items = results });

        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred...", error = ex.Message });
        }
    }


    [HttpGet("malfunction-frequency")]
    public async Task<IActionResult> GetMalfunctionFrequency(
        [FromQuery] string brand = null,
        [FromQuery] string model = null,
        [FromQuery] string sortColumn = "TotalRepairs",
        [FromQuery] string sortDirection = "asc")
    {
        try
        {
            var query = _context.MalfunctionFrequency.AsQueryable();

            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(m => m.Brand.Contains(brand));
            }

            if (!string.IsNullOrEmpty(model))
            {
                query = query.Where(m => m.Model.Contains(model));
            }

            query = ApplySorting(query, sortColumn, sortDirection);
            var results = await query.ToListAsync();


            return Ok(new { items = results });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred...", error = ex.Message });
        }
    }


    private IQueryable<T> ApplySorting<T>(IQueryable<T> query, string sortColumn, string sortDirection)
    {
        if (string.IsNullOrEmpty(sortColumn) || typeof(T).GetProperty(sortColumn) == null)
        {
            return query; 
        }

        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, sortColumn);
        var lambda = Expression.Lambda(property, parameter);


        var method = sortDirection == "desc" ? "OrderByDescending" : "OrderBy";

        var result = typeof(Queryable).GetMethods().Single(
            methodInfo =>
                methodInfo.Name == method
                && methodInfo.IsGenericMethodDefinition
                && methodInfo.GetGenericArguments().Length == 2
                && methodInfo.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), property.Type)
            .Invoke(null, new object[] { query, lambda });
        return (IQueryable<T>)result;

    }

}