using Naitrust.Domain.Models.Enums.Communication;

namespace Naitrust.Application.ExternalServices;

public interface IEmailProviderFactory
{
    IEmailProvider GetProvider(EmailProviderId providerId);
}
