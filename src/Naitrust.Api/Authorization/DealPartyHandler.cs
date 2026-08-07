using Microsoft.AspNetCore.Authorization;

namespace Naitrust.Api.Authorization;

public class DealPartyHandler : AuthorizationHandler<DealPartyRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DealPartyRequirement requirement)
    {
        return Task.CompletedTask; // TODO: Check if user is party to deal
    }
}
