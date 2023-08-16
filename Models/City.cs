//In the name of Allah

namespace SAGA.Models;

public class City
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsProcessed { get; set; }
    public bool IsActive { get; set; }
}//class

public class CityDelete
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
}//class



