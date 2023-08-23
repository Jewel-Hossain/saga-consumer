//In the name of Allah
public class CityConsumer : IConsumer<AddCity>
{
    private readonly InMemoryDbContext _dbContext;
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public CityConsumer(InMemoryDbContext dbContext,ISendEndpointProvider sendEndpointProvider)
    {
        _dbContext = dbContext;
        _sendEndpointProvider = sendEndpointProvider;
    }

    public async Task Consume(ConsumeContext<AddCity> context)
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


            _dbContext.Cities.Add(city);
            await _dbContext.SaveChangesAsync();

            var message = new CityInsertionSucceeded
            {
                CorrelationId = context.Message.CorrelationId,
                CityId = context.Message.City.Id
            };

            var successQueueEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:city-added-success-queue"));
            await successQueueEndpoint.Send(message);

        }//try
        catch (Exception ex)
        {

            var message = new CityInsertionFailed
            {
                CorrelationId = context.Message.CorrelationId,
                CityId = context.Message.City.Id
            };

            var failedQueueEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:city-added-failed-queue"));
            await failedQueueEndpoint.Send(message);
        }//catch
    }//func
}//class
