using Naitrust.Domain.Models.Enums.Transactions;

namespace Naitrust.Application.Services.Utility;

public static class PaymentStateMachine
{
    public static bool CanTransition(PaymentStatus from, PaymentStatus to) =>
        throw new NotImplementedException();
}
