using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using NSwag;

namespace TicketingSystem.WebApi;

public static class Registry
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddAuthentication();

        services.AddAuthorization();

        services.AddFastEndpoints(config =>
        {
            config.IncludeAbstractValidators = true;
        });

        services.SwaggerDocument(o =>
        {
            o.EnableJWTBearerAuth = false;
            o.DocumentSettings = s =>
            {
                s.DocumentName = "Ticketing System";
                s.Title = "Web API";
                s.Version = "v1.0";
                s.AddAuth("Bearer", new()
                {
                    Type = OpenApiSecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                });
            };
        });

        services.AddProblemDetails();

        return services;
    }

    public static void UseWebApi(this WebApplication app)
    {
        app.UseAuthentication();

        app.UseAuthorization();

        app.UseHttpsRedirection();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwaggerGen();
        }

        app.UseFastEndpoints(c =>
        {
            c.Endpoints.RoutePrefix = "api";
        });

        app.UseDefaultExceptionHandler();
    }
}
