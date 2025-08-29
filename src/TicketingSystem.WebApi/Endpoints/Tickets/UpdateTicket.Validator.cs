using FluentValidation;
using TicketingSystem.Domain.Enums;

namespace TicketingSystem.WebApi.Endpoints.Tickets;

public class UpdateTicketValidator : AbstractValidator<UpdateTicketRequest>
{
    public UpdateTicketValidator()
    {
        RuleFor(x => x.NewStatusId)
            .Must(BeAValidStatus)
            .WithMessage("Invalid ticket status value.");
    }

    private bool BeAValidStatus(short? statusId)
    {
        if (statusId is null)
            return true;

        return TicketStatus.TryFromValue(statusId.Value, out _);
    }
}
