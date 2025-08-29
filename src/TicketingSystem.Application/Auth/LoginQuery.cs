using ErrorOr;
using MediatR;
using TicketingSystem.Application.Common.Security;
using TicketingSystem.Application.Repositories;

namespace TicketingSystem.Application.Auth;

public record LoginQuery(string Email, string Password) : IRequest<ErrorOr<string>>;

public class LoginQueryHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<LoginQuery, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null)
        {
            return Error.NotFound(
                code: "User.NotFound",
                description: "Email is invalid!"
            );
        }

        var passwordValid = passwordHasher.VerifyPassword(request.Password, user.PasswordHash);
        if (!passwordValid)
        {
            return Error.Unauthorized(
                code: "User.InvalidPassword",
                description: "Password is invalid!"
            );
        }

        var token = jwtTokenGenerator.GenerateToken(user.Id, user.FullName, user.Email, [user.Role]);

        return token;
    }
}