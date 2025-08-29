using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using Throw;
using TicketingSystem.Application.Common.Security;
using TicketingSystem.Application.Models;

namespace TicketingSystem.Infrastructure.Security;

public class CurrentUserProvider(IHttpContextAccessor _httpContextAccessor) : ICurrentUserProvider
{
    public CurrentUser GetCurrentUser()
    {
        _httpContextAccessor.HttpContext.ThrowIfNull();

        var id = Guid.Parse(GetSingleClaimValue(ClaimTypes.NameIdentifier));
        var roles = GetClaimValues(ClaimTypes.Role);
        var fullName = GetSingleClaimValue(ClaimTypes.Name);
        var email = GetSingleClaimValue(ClaimTypes.Email);

        return new CurrentUser(id, fullName, email, roles);
    }

    private List<string> GetClaimValues(string claimType) =>
        _httpContextAccessor.HttpContext!.User.Claims
            .Where(claim => claim.Type == claimType)
            .Select(claim => claim.Value)
            .ToList();

    private string GetSingleClaimValue(string claimType) =>
        _httpContextAccessor.HttpContext!.User.Claims
            .Single(claim => claim.Type == claimType)
            .Value;
}