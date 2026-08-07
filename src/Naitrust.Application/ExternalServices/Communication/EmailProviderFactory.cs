using Microsoft.Extensions.DependencyInjection;
using Naitrust.Application.ExternalServices.Communication.Resend;
using Naitrust.Domain.Models.Enums.Communication;

namespace Naitrust.Application.ExternalServices.Communication;

public class EmailProviderFactory : IEmailProviderFactory
{
    private readonly IServiceProvider _serviceProvider;

    public EmailProviderFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IEmailProvider GetProvider(EmailProviderId providerId) => providerId switch
    {
        EmailProviderId.Resend => _serviceProvider.GetRequiredService<ResendEmailProvider>(),
        _ => throw new NotSupportedException($"Email provider '{providerId}' is not supported.")
    };
}
