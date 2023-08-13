public class InsertCityConsumer<TDbContext> : IConsumer<InsertCity> where TDbContext : DbContext
{
    private readonly TDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public InsertCityConsumer(TDbContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<InsertCity> context)
    {
        try
        {
            var city = new City
            {
                Id = context.Message.City.Id,
                Name = context.Message.City.Name,
                IsProcessed = true
            };

            throw new Exception();

            _dbContext.Add(city);
            await _dbContext.SaveChangesAsync();

            var message = new CityInsertionSucceeded
            {
                CorrelationId = context.Message.CorrelationId,
                CityId = context.Message.City.Id
            };

            await _publishEndpoint.Publish(message);
        }
        catch (Exception ex)
        {
            await _publishEndpoint.Publish(new CityInsertionFailed
            {
                CorrelationId = context.Message.CorrelationId,
                CityId = context.Message.City.Id
            });
        }
    }
}
