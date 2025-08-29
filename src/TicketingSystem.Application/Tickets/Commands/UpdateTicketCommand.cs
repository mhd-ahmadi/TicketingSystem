using MediatR;
using TicketingSystem.Application.Common;
using TicketingSystem.Application.Common.Security;
using TicketingSystem.Application.Repositories;
using TicketingSystem.Domain.Entities;
using TicketingSystem.Domain.Enums;
using TicketingSystem.Domain.Exceptions;

namespace TicketingSystem.Application.Tickets.Commands;

public record UpdateTicketCommand(Guid Id, Guid? AssignToUserId, TicketStatus? NewStatus) : IRequest;

internal class UpdateTicketCommandHandler(
    ITicketRepository ticketRepository,
    IDateTimeProvider dateTimeProvider,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateTicketCommand>
{
    public async Task Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByIdAsync(request.Id, cancellationToken) ??
            throw new EntityNotFoundException(nameof(Ticket), request.Id);

        if (request.AssignToUserId.HasValue)
        {
            ticket.AssignTo(request.AssignToUserId.Value, dateTimeProvider.Now);
        }

        if (request.NewStatus is not null)
        {
            ticket.SetStatus(request.NewStatus, dateTimeProvider.Now);
        }

        ticketRepository.Update(ticket);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
