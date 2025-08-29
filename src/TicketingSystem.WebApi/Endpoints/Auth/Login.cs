using FastEndpoints;
using MediatR;
using TicketingSystem.Application.Auth;

namespace TicketingSystem.WebApi.Endpoints.Auth;

public class Login(ISender sender) : Endpoint<LoginRequest, LoginResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post(LoginRequest.Route);

        Summary(s => s.Summary = "Authenticate user and return JWT");
        Description(x => x.WithTags("Auth"));
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
         var loginResult = await sender.Send(
             new LoginQuery(req.Email, req.Password), ct);

        if (loginResult.IsError)
        {
            foreach (var error in loginResult.Errors)
            {
                AddError(message: error.Description);
            }
            ThrowIfAnyErrors(401);
            return;
        }

        await Send.OkAsync(new LoginResponse(loginResult.Value), ct);
    }
}
