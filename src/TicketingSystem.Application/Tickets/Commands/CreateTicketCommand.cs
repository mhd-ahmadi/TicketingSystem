using MediatR;
using TicketingSystem.Application.Common;
using TicketingSystem.Application.Common.Security;
using TicketingSystem.Application.Repositories;
using TicketingSystem.Domain.Entities;
using TicketingSystem.Domain.Enums;

namespace TicketingSystem.Application.Tickets.Commands;

public record CreateTicketCommand(string Title, string Description, TicketPriority Priority) : IRequest<Guid>;

public class CreateTicketCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IDateTimeProvider dateTimeProvider,
    ITicketRepository ticketRepository, 
    IUnitOfWork unitOfWork) : IRequestHandler<CreateTicketCommand, Guid>
{
    public async Task<Guid> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserProvider.GetCurrentUser().Id;
        var ticket = Ticket.Create(request.Title, request.Description, request.Priority, currentUserId, dateTimeProvider.Now);
        await ticketRepository.AddAsync(ticket, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ticket.Id;
    }
}
