public class InsertCuisineConsumer : IConsumer<InsertCuisine>
{
    private readonly InMemoryDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public InsertCuisineConsumer(InMemoryDbContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<InsertCuisine> context)
    {
        try
        {
            var Cuisine = new Cuisine
            {
                Id = context.Message.Cuisine.Id,
                Name = context.Message.Cuisine.Name,
                IsProcessed = true
            };


            _dbContext.Cuisines.Add(Cuisine);
            await _dbContext.SaveChangesAsync();

            var message = new CuisineInsertionSucceeded
            {
                CorrelationId = context.Message.CorrelationId,
                CuisineId = context.Message.Cuisine.Id
            };

            await _publishEndpoint.Publish(message);
        }
        catch (Exception ex)
        {
            await _publishEndpoint.Publish(new CuisineInsertionFailed
            {
                CorrelationId = context.Message.CorrelationId,
                CuisineId = context.Message.Cuisine.Id
            });
        }
    }
}
