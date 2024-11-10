using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using Npgsql;


[ApiController]
[Route("api/[controller]")]
public class GenericController<T> : ControllerBase where T : class
{
    private readonly CarWorkshopContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericController(CarWorkshopContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<T>>> GetAll(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string sortColumn = "",
    [FromQuery] string sortDirection = "asc",
    [FromQuery] Dictionary<string, string> filters = null)
{
    IQueryable<T> query = _dbSet;

    // Apply filters
    if (filters != null)
    {
        foreach (var filter in filters)
        {
            var propertyName = filter.Key.Replace("_min", "").Replace("_max", "").Replace("_in", "");
            var property = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property != null)
            {
                var exprParameter = Expression.Parameter(typeof(T), "e");
                var exprPropertyAccess = Expression.Property(exprParameter, property);

                if (property.PropertyType == typeof(string))
                {
                    if (!filter.Key.Contains("_min") && !filter.Key.Contains("_max"))
                    {
                        if (!string.IsNullOrEmpty(filter.Value))
                        {
                            var values = filter.Value.Split(',')
                                .Select(v => v.Trim().ToLower())
                                .ToArray();

                            Expression orExpression = null;

                            foreach (var value in values)
                            {
                                var constant = Expression.Constant(value);
                                
                                var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                                var toLowerExpression = Expression.Call(exprPropertyAccess, toLowerMethod);

                                var containsExpression = Expression.Call(toLowerExpression, typeof(string).GetMethod("Contains", new[] { typeof(string) }), constant);
                                orExpression = orExpression == null ? containsExpression : Expression.OrElse(orExpression, containsExpression);
                            }

                            var lambda = Expression.Lambda<Func<T, bool>>(orExpression, exprParameter);
                            query = query.Where(lambda);
                        }
                    }
                }
                else if (property.PropertyType.IsValueType || property.PropertyType == typeof(string))
                {
                    if (filter.Key.EndsWith("_min"))
                    {
                        var minValue = Convert.ChangeType(filter.Value, property.PropertyType);
                        var greaterThanOrEqualExpression = Expression.GreaterThanOrEqual(exprPropertyAccess, Expression.Constant(minValue));
                        var lambda = Expression.Lambda<Func<T, bool>>(greaterThanOrEqualExpression, exprParameter);
                        query = query.Where(lambda);
                    }
                    else if (filter.Key.EndsWith("_max"))
                    {
                        var maxValue = Convert.ChangeType(filter.Value, property.PropertyType);
                        var lessThanOrEqualExpression = Expression.LessThanOrEqual(exprPropertyAccess, Expression.Constant(maxValue));
                        var lambda = Expression.Lambda<Func<T, bool>>(lessThanOrEqualExpression, exprParameter);
                        query = query.Where(lambda);
                    }
                    else if (filter.Key.EndsWith("_in"))
                    {
                        var values = filter.Value.Split(',')
                            .Select(v => Convert.ChangeType(v.Trim(), property.PropertyType))
                            .ToList();

                        var listType = typeof(List<>).MakeGenericType(property.PropertyType);
                        var list = Activator.CreateInstance(listType);
                        var addMethod = listType.GetMethod("Add");

                        foreach (var value in values)
                        {
                            addMethod.Invoke(list, new[] { value });
                        }

                        var containsMethod = listType.GetMethod("Contains", new[] { property.PropertyType });
                        var containsExpression = Expression.Call(Expression.Constant(list), containsMethod, exprPropertyAccess);
                        var lambda = Expression.Lambda<Func<T, bool>>(containsExpression, exprParameter);
                        query = query.Where(lambda);
                    }
                    else
                    {
                        var exactValue = Convert.ChangeType(filter.Value, property.PropertyType);
                        var equalsExpression = Expression.Equal(exprPropertyAccess, Expression.Constant(exactValue));
                        var lambda = Expression.Lambda<Func<T, bool>>(equalsExpression, exprParameter);
                        query = query.Where(lambda);
                    }
                }
            }
        }
    }

        if (!string.IsNullOrEmpty(sortColumn))
        {
            var property = typeof(T).GetProperty(sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property != null)
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var propertyAccess = Expression.Property(parameter, property);
                var lambda = Expression.Lambda(propertyAccess, parameter);

                string methodName = sortDirection.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";
                query = query.Provider.CreateQuery<T>(
                    Expression.Call(
                        typeof(Queryable),
                        methodName,
                        new Type[] { typeof(T), property.PropertyType },
                        query.Expression,
                        Expression.Quote(lambda)
                    ));
            }
        }

        var totalItems = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<T>
        {
            Items = items,
            TotalItems = totalItems,
            Page = page,
            PageSize = pageSize
        };
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<T>> GetById(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
        {
            return NotFound();
        }
        return entity;
    }

[HttpPost]
public async Task<ActionResult<T>> Create([FromBody] T entity)
{
    try
    {
        var entityType = typeof(T);

        foreach (var prop in entityType.GetProperties())
        {
            var foreignKeyAttr = prop.GetCustomAttribute<ForeignKeyAttribute>();
            if (foreignKeyAttr != null)
            {
                var foreignKeyId = prop.GetValue(entity); 
                var navigationProperty = entityType.GetProperty(foreignKeyAttr.Name); 

                if (navigationProperty != null && foreignKeyId != null)
                {
                    var foreignEntityType = navigationProperty.PropertyType;

                    var dbSet = _context.GetType().GetMethod("Set").MakeGenericMethod(foreignEntityType).Invoke(_context, null);
                    var foreignEntity = await (Task<object>)dbSet.GetType().GetMethod("FindAsync").Invoke(dbSet, new object[] { new object[] { foreignKeyId } });

                    if (foreignEntity == null)
                    {
                        return BadRequest(new { message = $"{navigationProperty.Name} с ID {foreignKeyId} не найден." });
                    }

                    navigationProperty.SetValue(entity, foreignEntity);
                }
            }
        }

        _context.Add(entity);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = GetEntityId(entity) }, entity);
    }
    catch (DbUpdateException ex)
    {
        if (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
        {
            return Conflict(new { message = $"Ошибка: Значение id '{GetEntityId(entity)}' уже существует." });
        }
        Console.WriteLine(ex);
        return StatusCode(500, new { message = "Ошибка при создании записи." });
    }
}


    [HttpPut("{id}")]
    public async Task<ActionResult<T>> Update(int id, T entity)
    {
        if (id != GetEntityId(entity))
        {
            return BadRequest(new { message = "Id в URL и теле запроса не совпадают." }); 
        }

        try
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(entity);
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505") 
            {
                return Conflict(new { message = $"Ошибка: Значение id '{GetEntityId(entity)}' уже существует." }); // Возвращаем 409 Conflict
            }
            Console.WriteLine(ex); 
            return StatusCode(500, new { message = "Ошибка при обновлении записи." });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
        {
            return NotFound();
        }

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool EntityExists(int id)
    {
        return _dbSet.Find(id) != null;
    }

    private int GetEntityId(T entity)
    {
        var keyProperty = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties[0];
        return (int)keyProperty.GetGetter().GetClrValue(entity);
    }
}

public class PagedResult<T>
{
    public List<T> Items { get; set; }
    public int TotalItems { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
