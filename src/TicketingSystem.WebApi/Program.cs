using FastEndpoints;
using TicketingSystem.Application;
using TicketingSystem.Infrastructure;
using TicketingSystem.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddPresentation();

var app = builder.Build();

await app.UseInfrastructureAsync();

app.UseWebApi();

app.Run();