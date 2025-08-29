using Microsoft.EntityFrameworkCore;
using TicketingSystem.Application.Repositories;
using TicketingSystem.Domain.Entities;
using TicketingSystem.Domain.Enums;

namespace TicketingSystem.Infrastructure.Persistence.Repositories;

public class TicketRepository(AppDbContext context) : ITicketRepository
{
    public async Task<Ticket?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await context.Tickets.FindAsync(keyValues: [id], cancellationToken: cancellationToken);

    public async Task<IReadOnlyCollection<Ticket>> GetListAsync(int offset, int limit, CancellationToken cancellationToken = default) =>
        await context.Tickets
            .AsNoTracking()
            .OrderByDescending(o => o.CreatedAt)
            .Skip(offset)
            .Take(limit)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<Ticket>> GetByUserIdAsync(Guid userId, int offset, int limit, CancellationToken cancellationToken = default) =>
        await context.Tickets
            .AsNoTracking()
            .Where(t => t.CreatedByUserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .Skip(offset)
            .Take(limit)
            .ToListAsync(cancellationToken);

    public async Task<int> GetTotalCountAsync(Guid createdByUserId, CancellationToken cancellationToken = default) =>
        await context.Tickets.AsNoTracking().Where(w => w.CreatedByUserId == createdByUserId).CountAsync(cancellationToken);

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default) =>
        await context.Tickets.AsNoTracking().CountAsync(cancellationToken);

    public async Task<Dictionary<TicketStatus, int>> GetTicketCountsByStatusAsync(CancellationToken cancellationToken = default)
    {
        return await context.Tickets
            .AsNoTracking()
            .GroupBy(t => t.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Status, x => x.Count, cancellationToken);
    }

    public async Task AddAsync(Ticket ticket, CancellationToken cancellationToken = default) => 
        await context.Tickets.AddAsync(ticket, cancellationToken);

    public void Update(Ticket ticket) => context.Tickets.Update(ticket);

    public void Remove(Ticket ticket) => context.Tickets.Remove(ticket);
}
