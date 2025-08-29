using Microsoft.EntityFrameworkCore;
using TicketingSystem.Application.Repositories;
using TicketingSystem.Domain.Entities;

namespace TicketingSystem.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await context.Users.FindAsync(keyValues: [id], cancellationToken: cancellationToken);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default) =>
        await context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public async Task AddAsync(User user, CancellationToken cancellationToken = default) => 
        await context.Users.AddAsync(user, cancellationToken);

    public void Update(User user) => context.Users.Update(user);

    public void Remove(User user) => context.Users.Remove(user);
}