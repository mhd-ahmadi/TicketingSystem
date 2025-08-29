using Microsoft.EntityFrameworkCore;
using TicketingSystem.Application.Common.Security;
using TicketingSystem.Domain.Entities;

namespace TicketingSystem.Infrastructure.Persistence;

internal interface IDbInitializer
{
    Task InitializeAsync();
}

internal class DbInitializer(
    AppDbContext context,
    IPasswordHasher passwordHasher,
    IDateTimeProvider dateTimeProvider) : IDbInitializer
{
    public async Task InitializeAsync()
    {
        await context.Database.MigrateAsync();

        if (!await context.Users.AnyAsync())
        {
            context.Users.AddRange(
            [
                User.CreateAdmin("Admin", "admin@site.com", passwordHasher.HashPassword("Ad@123"), dateTimeProvider.Now),
                User.CreateEmployee("Emp", "emp@site.com", passwordHasher.HashPassword("Em@123"), dateTimeProvider.Now)
            ]);
            await context.SaveChangesAsync();
        }
    }
}