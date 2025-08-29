using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketingSystem.Application.Common;
using TicketingSystem.Application.Common.Security;
using TicketingSystem.Application.Repositories;
using TicketingSystem.Application.Services;
using TicketingSystem.Infrastructure.Middlewares;
using TicketingSystem.Infrastructure.Persistence;
using TicketingSystem.Infrastructure.Persistence.Interceptors;
using TicketingSystem.Infrastructure.Persistence.Repositories;
using TicketingSystem.Infrastructure.Security;
using TicketingSystem.Infrastructure.Security.TokenGenerator;
using TicketingSystem.Infrastructure.Security.TokenValidation;
using TicketingSystem.Infrastructure.Services;

namespace TicketingSystem.Infrastructure;

public static class Registry
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHttpContextAccessor()
            .AddServices()
            .AddAuthentication(configuration)
            .AddAuthorization()
            .AddPersistence();

        return services;
    }

    public static async Task<IApplicationBuilder> UseInfrastructureAsync(this IApplicationBuilder app)
    {
        app.UseMiddleware<EventualConsistencyMiddleware>();

        using (var scope = app.ApplicationServices.CreateScope())
        {
            var initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await initializer.InitializeAsync();
        }

        return app;
    }


    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<DomainEventsInterceptor>();
        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            var interceptor = serviceProvider.GetRequiredService<DomainEventsInterceptor>();
            options
                .UseSqlite("Data Source=TicketingSystem.sqlite")
                .AddInterceptors(interceptor);
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDbInitializer, DbInitializer>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();

        return services;
    }

    private static IServiceCollection AddAuthorization(this IServiceCollection services)
    {
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
        
        services.AddTransient<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IPolicyEnforcer, PolicyEnforcer>();

        return services;
    }

    private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Section));

        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        services
            .ConfigureOptions<JwtBearerTokenValidationConfiguration>()
            .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        return services;
    }
}