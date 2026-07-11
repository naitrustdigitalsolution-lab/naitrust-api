namespace Naitrust.Application.ExternalServices.Communication;

public class TermiiSmsService : ISmsService
{
    public Task SendSmsAsync(string to, string message, CancellationToken ct = default) =>
        throw new NotImplementedException();

    public Task SendOtpAsync(string to, CancellationToken ct = default) =>
        throw new NotImplementedException();
}
