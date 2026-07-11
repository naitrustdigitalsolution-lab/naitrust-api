using Naitrust.Domain.Models.Enums.Disputes;

namespace Naitrust.Application.Services.Utility;

public static class DisputeStateMachine
{
    public static bool CanTransition(DisputeStatus from, DisputeStatus to) =>
        throw new NotImplementedException();
}
