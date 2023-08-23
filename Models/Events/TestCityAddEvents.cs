//In the name of Allah

namespace SAGA.Models;

public class AddCity : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; set; }
    public City City { get; set; }
}