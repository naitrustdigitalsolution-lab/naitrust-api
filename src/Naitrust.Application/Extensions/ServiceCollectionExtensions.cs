using Microsoft.Extensions.DependencyInjection;
using Naitrust.Application.Configurations;

namespace Naitrust.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        => services.AddApplicationLayer();
}
