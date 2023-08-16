public class UpdateCityConsumer : IConsumer<UpdateCity>
{
    private readonly InMemoryDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdateCityConsumer(InMemoryDbContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<UpdateCity> context)
    {
        try
        {
            City city = await _dbContext.Cities.FindAsync(context.Message.City.Id);


            city.Id = context.Message.City.Id;
            city.Name = context.Message.City.Name;
            city.CreatedAt = context.Message.City.CreatedAt;
            city.IsProcessed = context.Message.City.IsProcessed;
            city.IsActive = context.Message.City.IsActive;



            _dbContext.Cities.Update(city);
            await _dbContext.SaveChangesAsync();

            var message = new CityUpdateSucceeded
            {
                CorrelationId = context.Message.CorrelationId,
                CityId = context.Message.City.Id
            };

            await _publishEndpoint.Publish(message);
        }
        catch (Exception ex)
        {
            await _publishEndpoint.Publish(new CityUpdateFailed
            {
                CorrelationId = context.Message.CorrelationId,
                CityId = context.Message.City.Id
            });
        }
    }
}
