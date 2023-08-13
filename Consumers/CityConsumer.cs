public class InsertCityConsumer : IConsumer<InsertCity>
{
    private readonly InMemoryDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public InsertCityConsumer(InMemoryDbContext dbContext, IPublishEndpoint publishEndpoint)
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


            _dbContext.Cities.Add(city);
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
