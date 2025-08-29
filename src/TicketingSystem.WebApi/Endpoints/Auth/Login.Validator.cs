using FastEndpoints;
using FluentValidation;

namespace TicketingSystem.WebApi.Endpoints.Auth
{
    public class LoginValidator : Validator<LoginRequest>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(4).WithMessage("Password must be at least 4 characters long.");
                //.Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                //.Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                //.Matches("[0-9]").WithMessage("Password must contain at least one number.")
                //.Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
        }
    }
}
