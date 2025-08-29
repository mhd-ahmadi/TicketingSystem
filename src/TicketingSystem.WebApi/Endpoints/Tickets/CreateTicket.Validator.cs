using FastEndpoints;
using FluentValidation;
using TicketingSystem.Application.Common.Extensions;
using TicketingSystem.Domain.Enums;

namespace TicketingSystem.WebApi.Endpoints.Tickets;

public class CreateTicketValidator : Validator<CreateTicketRequest>
{
    public CreateTicketValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.");

        RuleFor(x => x.PriorityId)
            .NotNull().WithMessage("Priority is required.")
            .IsInEnum(typeof(TicketPriority)).WithMessage("Invalid priority selected.");
    }
}
