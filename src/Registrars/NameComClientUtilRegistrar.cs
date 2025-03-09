using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.NameCom.Client.Abstract;
using Soenneker.Utils.HttpClientCache.Registrar;

namespace Soenneker.NameCom.Client.Registrars;

/// <summary>
/// A .NET thread-safe singleton HttpClient for Name.com
/// </summary>
public static class NameComClientUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="INameComClientUtil"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddNameComClientUtilAsSingleton(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton();
        services.TryAddSingleton<INameComClientUtil, NameComClientUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="INameComClientUtil"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddNameComClientUtilAsScoped(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton();
        services.TryAddScoped<INameComClientUtil, NameComClientUtil>();

        return services;
    }
}
