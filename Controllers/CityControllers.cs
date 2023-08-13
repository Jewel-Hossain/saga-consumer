//In the name of Allah
namespace SAGA.Controllers;

[ApiController]
[Route("[controller]")]
public class CityController : ControllerBase
{
    private readonly InMemoryDbContext _dbContext;

    public CityController(InMemoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var cities = await _dbContext.Set<City>().ToListAsync();
        return Ok(cities);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var city = await _dbContext.FindAsync<City>(id);
        if (city == null)
        {
            return NotFound();
        }
        return Ok(city);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] City city)
    {
        city.Id = Guid.NewGuid();

        _dbContext.Add(city);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = city.Id }, city);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] City city)
    {
        if (id != city.Id)
        {
            return BadRequest();
        }

        _dbContext.Entry(city).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var city = await _dbContext.FindAsync<City>(id);
        if (city == null)
        {
            return NotFound();
        }

        _dbContext.Remove(city);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}
