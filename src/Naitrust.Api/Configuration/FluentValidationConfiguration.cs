using FluentValidation;
using FluentValidation.AspNetCore;

namespace Naitrust.Api.Configuration;

public static class FluentValidationConfiguration
{
    public static IServiceCollection AddFluentValidationServices(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<Naitrust.Application.Validators.Auth.RegisterRequestValidator>();

        return services;
    }
}
