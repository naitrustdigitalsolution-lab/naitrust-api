using Naitrust.Domain.Models.Enums.Verification;

namespace Naitrust.Application.Services.Utility;

public static class VerificationStateMachine
{
    public static bool CanTransition(VerificationStatus from, VerificationStatus to) =>
        throw new NotImplementedException();
}
