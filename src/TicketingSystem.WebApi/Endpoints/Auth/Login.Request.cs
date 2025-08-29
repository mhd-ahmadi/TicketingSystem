namespace TicketingSystem.WebApi.Endpoints.Auth;

public record LoginRequest(string Email, string Password)
{
    public const string Route = "/auth/login";
}
