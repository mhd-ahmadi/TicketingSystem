using Mapster;
using Microsoft.Extensions.DependencyInjection;
using TicketingSystem.Application.Common.Behaviors;

namespace TicketingSystem.Application;

public static class Registry
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(Registry).Assembly);

            options.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
            options.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        //services.AddValidatorsFromAssemblyContaining(typeof(Registry));
        TypeAdapterConfig.GlobalSettings.Scan(typeof(Registry).Assembly);
        return services;
    }
}