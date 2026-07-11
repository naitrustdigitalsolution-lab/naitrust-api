using Naitrust.Domain.Models.Enums.Transactions;

namespace Naitrust.Application.Services.Utility;

public static class TransactionStateMachine
{
    public static bool CanTransition(TransactionStatus from, TransactionStatus to) =>
        throw new NotImplementedException();

    public static TransactionStatus[] GetAllowedTransitions(TransactionStatus current) =>
        throw new NotImplementedException();
}
