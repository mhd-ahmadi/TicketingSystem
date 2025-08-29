using MediatR;
using TicketingSystem.Application.Common;
using TicketingSystem.Application.Repositories;
using TicketingSystem.Domain.Entities;
using TicketingSystem.Domain.Exceptions;

namespace TicketingSystem.Application.Tickets.Commands;

public record DeleteTicketCommand(Guid TicketId) : IRequest;

internal class DeleteTicketCommandHandler(ITicketRepository ticketRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteTicketCommand>
{
    public async Task Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByIdAsync(request.TicketId, cancellationToken) ??
            throw new EntityNotFoundException(nameof(Ticket), request.TicketId);

        ticketRepository.Remove(ticket);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
