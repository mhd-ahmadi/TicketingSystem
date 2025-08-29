using TicketingSystem.Domain.Entities;
using TicketingSystem.Domain.Enums;

namespace TicketingSystem.Application.Repositories;

public interface ITicketRepository
{
    Task<Ticket?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Ticket>> GetListAsync(int offset, int limit, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Ticket>> GetByUserIdAsync(Guid userId, int offset, int limit, CancellationToken cancellationToken = default);
    Task AddAsync(Ticket ticket, CancellationToken cancellationToken = default);
    void Update(Ticket ticket);
    void Remove(Ticket ticket);
    Task<int> GetTotalCountAsync(Guid createdByUserId, CancellationToken cancellationToken = default);
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);
    Task<Dictionary<TicketStatus, int>> GetTicketCountsByStatusAsync(CancellationToken cancellationToken = default);
}
