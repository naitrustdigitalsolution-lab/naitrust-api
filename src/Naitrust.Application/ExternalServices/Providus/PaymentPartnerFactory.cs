using Naitrust.Domain.Models.Enums.Payments;

namespace Naitrust.Application.ExternalServices.Providus;

public class PaymentPartnerFactory : IPaymentPartnerFactory
{
    public IPaymentPartner GetPartner(PaymentPartnerId partnerId) =>
        throw new NotImplementedException();
}
