using Mapster;
using TicketingSystem.Application.Tickets.Dto;
using TicketingSystem.Domain.Entities;

namespace TicketingSystem.Application.Tickets.Mappings;

public class TicketMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Ticket, GetTicketQueryResultDto>()
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.Priority, src => src.Priority)
            .Map(dest => dest.AssignedToUserId, src => src.AssignedToUserId)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.UpdatedAt, src => src.UpdatedAt);
    }
}