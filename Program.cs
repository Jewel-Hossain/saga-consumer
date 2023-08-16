//In the name of Allah

global using MassTransit;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using SAGA.Models;
global using SAGA.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<InMemoryDbContext>(options => options.UseInMemoryDatabase("service-b-db"));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<InsertCityConsumer>();
    x.AddConsumer<UpdateCityConsumer>();
    x.AddConsumer<DeleteCityConsumer>();
    x.AddConsumer<InsertCuisineConsumer>();

    x.UsingRabbitMq((context, config) =>
    {
        var connection = new Uri("amqp://admin:admin2023@18.138.164.11:5672");
        config.Host(connection);

        config.ReceiveEndpoint("city-add-consumer", e => e.ConfigureConsumer<InsertCityConsumer>(context));
        config.ReceiveEndpoint("city-update-consumer", e => e.ConfigureConsumer<UpdateCityConsumer>(context));
        config.ReceiveEndpoint("city-delete-consumer", e => e.ConfigureConsumer<DeleteCityConsumer>(context));
        config.ReceiveEndpoint("cuisine-add-consumer", e => e.ConfigureConsumer<InsertCuisineConsumer>(context));
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
