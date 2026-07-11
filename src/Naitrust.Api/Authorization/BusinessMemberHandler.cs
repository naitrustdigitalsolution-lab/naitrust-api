using Microsoft.AspNetCore.Authorization;

namespace Naitrust.Api.Authorization;

public class BusinessMemberHandler : AuthorizationHandler<BusinessMemberRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BusinessMemberRequirement requirement)
    {
        return Task.CompletedTask; // TODO: Check if user is a member of the business
    }
}
