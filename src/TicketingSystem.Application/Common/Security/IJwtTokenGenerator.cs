namespace TicketingSystem.Application.Common.Security;

public interface IJwtTokenGenerator
{
    string GenerateToken(
        Guid id,
        string fullName,
        string email,
        List<string> roles
        );
}