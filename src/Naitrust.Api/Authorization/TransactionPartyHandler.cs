using Microsoft.AspNetCore.Authorization;

namespace Naitrust.Api.Authorization;

public class TransactionPartyHandler : AuthorizationHandler<TransactionPartyRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TransactionPartyRequirement requirement)
    {
        return Task.CompletedTask; // TODO: Check if user is party to transaction
    }
}
