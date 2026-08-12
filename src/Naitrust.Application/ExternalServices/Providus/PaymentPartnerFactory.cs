using Microsoft.Extensions.DependencyInjection;
using Naitrust.Application.ExternalServices.Anchor;
using Naitrust.Domain.Models.Enums.Payments;

namespace Naitrust.Application.ExternalServices.Providus;

public class PaymentPartnerFactory : IPaymentPartnerFactory
{
    private readonly IServiceProvider _serviceProvider;

    public PaymentPartnerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IPaymentPartner GetPartner(PaymentPartnerId partnerId) => partnerId switch
    {
        PaymentPartnerId.Anchor => _serviceProvider.GetRequiredService<AnchorPaymentPartner>(),
        _ => throw new NotSupportedException($"Payment partner '{partnerId}' is not yet integrated.")
    };
}
