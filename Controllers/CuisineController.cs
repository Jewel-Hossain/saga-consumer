//In the name of Allah
namespace SAGA.Controllers;

[ApiController]
[Route("[controller]")]
public class CuisineController : ControllerBase
{
    private readonly InMemoryDbContext _dbContext;

    public CuisineController(InMemoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var cities = await _dbContext.Set<Cuisine>().ToListAsync();
        return Ok(cities);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var Cuisine = await _dbContext.FindAsync<Cuisine>(id);
        if (Cuisine == null)
        {
            return NotFound();
        }
        return Ok(Cuisine);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Cuisine Cuisine)
    {
        Cuisine.Id = Guid.NewGuid();

        _dbContext.Add(Cuisine);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = Cuisine.Id }, Cuisine);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] Cuisine Cuisine)
    {
        if (id != Cuisine.Id)
        {
            return BadRequest();
        }

        _dbContext.Entry(Cuisine).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var Cuisine = await _dbContext.FindAsync<Cuisine>(id);
        if (Cuisine == null)
        {
            return NotFound();
        }

        _dbContext.Remove(Cuisine);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}
