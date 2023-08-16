public class DeleteCityConsumer : IConsumer<DeleteCity>
{
    private readonly InMemoryDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public DeleteCityConsumer(InMemoryDbContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<DeleteCity> context)
    {
        try
        {
            City city = await _dbContext.Cities.FindAsync(context.Message.CityDelete.Id);

            city.IsActive = context.Message.CityDelete.IsActive;


            _dbContext.Cities.Update(city);
            await _dbContext.SaveChangesAsync();

            var message = new CityDeleteSucceeded
            {
                CorrelationId = context.Message.CorrelationId,
                CityId = context.Message.CityDelete.Id
            };

            await _publishEndpoint.Publish(message);
        }
        catch (Exception ex)
        {
            await _publishEndpoint.Publish(new CityDeleteFailed
            {
                CorrelationId = context.Message.CorrelationId,
                CityId = context.Message.CityDelete.Id
            });
        }
    }
}
