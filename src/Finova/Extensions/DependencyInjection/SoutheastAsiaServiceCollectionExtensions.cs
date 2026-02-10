using System.Reflection;
using Finova.Core.Iban;
using Finova.Core.Identifiers;
using Finova.Core.Vat;
using Finova.Services.Adapters;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for setting up Southeast Asia validators in an <see cref="IServiceCollection" />.
/// </summary>
public static class SoutheastAsiaServiceCollectionExtensions
{
    /// <summary>
    /// Adds Southeast Asia validators to the specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
    public static IServiceCollection AddFinovaSoutheastAsia(this IServiceCollection services)
    {
        var assembly = Assembly.GetAssembly(typeof(SoutheastAsiaServiceCollectionExtensions)) ?? Assembly.GetExecutingAssembly();

        services.RegisterValidatorsFromNamespace(
            assembly,
            "Finova.Countries.SoutheastAsia",
            (s, type) =>
            {
                if (typeof(IIbanValidator).IsAssignableFrom(type))
                {
                    s.AddSingleton<IBankAccountValidator>(sp =>
                        new IbanBankAccountAdapter((IIbanValidator)sp.GetRequiredService(type)));
                }
            },
            typeof(ITaxIdValidator),
            typeof(IVatValidator),
            typeof(INationalIdValidator),
            typeof(IBankRoutingValidator),
            typeof(IBankAccountValidator),
            typeof(IIbanValidator),
            typeof(IBbanValidator)
        );

        return services;
    }
}
