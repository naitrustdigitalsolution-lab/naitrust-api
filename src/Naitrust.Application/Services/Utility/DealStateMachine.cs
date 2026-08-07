using Naitrust.Domain.Models.Enums.Transactions;

namespace Naitrust.Application.Services.Utility;

public static class DealStateMachine
{
    public static bool CanTransition(DealStatus from, DealStatus to) =>
        throw new NotImplementedException();

    public static DealStatus[] GetAllowedTransitions(DealStatus current) =>
        throw new NotImplementedException();
}
