using Naitrust.Domain.Models.Enums.Payments;

namespace Naitrust.Application.ExternalServices;

public interface IPaymentPartnerFactory
{
    IPaymentPartner GetPartner(PaymentPartnerId partnerId);
}
