using System.Collections.Concurrent;
using Finova.Core.Common;
using Finova.Core.Iban;
using Finova.Countries.NorthAmerica.Barbados.Validators;
using Finova.Countries.NorthAmerica.CostaRica.Validators;
using Finova.Countries.NorthAmerica.DominicanRepublic.Validators;
using Finova.Countries.NorthAmerica.ElSalvador.Validators;
using Finova.Countries.NorthAmerica.Guatemala.Validators;
using Finova.Countries.NorthAmerica.Honduras.Validators;
using Finova.Countries.NorthAmerica.Nicaragua.Validators;
using Finova.Countries.NorthAmerica.SaintLucia.Validators;
using Finova.Countries.NorthAmerica.VirginIslandsBritish.Validators;
using Finova.Countries.SouthAmerica.Brazil.Validators;
using Finova.Countries.SouthAmerica.FalklandIslands.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Services;

/// <summary>
/// Unified validator for American IBANs (North and South).
/// Delegates validation to specific country validators based on the country code prefix.
/// </summary>
public class AmericasIbanValidator : IIbanValidator
{
    private static readonly ConcurrentDictionary<string, IIbanValidator> _staticValidators = new();

    private readonly IServiceProvider? _serviceProvider;
    private IEnumerable<IIbanValidator>? _validators;

    public AmericasIbanValidator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public AmericasIbanValidator()
    {
        _serviceProvider = null;
    }

    private IEnumerable<IIbanValidator> GetValidators()
    {
        if (_validators == null && _serviceProvider != null)
        {
            _validators = _serviceProvider.GetServices<IIbanValidator>()
                                          .Where(v => v.GetType() != typeof(AmericasIbanValidator));
        }
        return _validators ?? Enumerable.Empty<IIbanValidator>();
    }

    public string CountryCode => "";

    public ValidationResult Validate(string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban) || iban.Length < 2)
        {
            return ValidateIban(iban);
        }

        string countryCode = iban[0..2].ToUpperInvariant();
        var validator = GetValidators().FirstOrDefault(v => v.CountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase));

        if (validator != null)
        {
            return validator.Validate(iban);
        }

        return ValidateIban(iban);
    }

    /// <summary>
    /// Validates an American IBAN using static logic.
    /// </summary>
    public static ValidationResult ValidateIban(string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (iban.Length < 2)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InvalidLength);
        }

        string country = IbanHelper.NormalizeIban(iban)[0..2].ToUpperInvariant();
        var validator = GetOrCreateValidator(country);

        return validator != null
            ? validator.Validate(iban)
            : ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountryOrInvalidIban);
    }

    private static IIbanValidator? GetOrCreateValidator(string countryCode)
    {
        if (_staticValidators.TryGetValue(countryCode, out var validator))
            return validator;

        validator = countryCode switch
        {
            "BB" => new BarbadosIbanValidator(),
            "BR" => new BrazilIbanValidator(),
            "CR" => new CostaRicaIbanValidator(),
            "DO" => new DominicanRepublicIbanValidator(),
            "SV" => new ElSalvadorIbanValidator(),
            "FK" => new FalklandIslandsIbanValidator(),
            "GT" => new GuatemalaIbanValidator(),
            "HN" => new HondurasIbanValidator(),
            "NI" => new NicaraguaIbanValidator(),
            "LC" => new SaintLuciaIbanValidator(),
            "VG" => new VirginIslandsBritishIbanValidator(),
            _ => null
        };

        if (validator != null)
            _staticValidators.TryAdd(countryCode, validator);

        return validator;
    }

    /// <inheritdoc/>
    public string FormatIban(string? iban) => IbanHelper.FormatIban(iban);

    /// <inheritdoc/>
    public string NormalizeIban(string? iban) => IbanHelper.NormalizeIban(iban);

    /// <inheritdoc/>
    public string GetCountryCode(string? iban) => IbanHelper.GetCountryCode(iban);

    /// <inheritdoc/>
    public int GetCheckDigits(string? iban) => IbanHelper.GetCheckDigits(iban);

    /// <inheritdoc/>
    public bool ValidateChecksum(string? iban) => IbanHelper.ValidateChecksum(iban);

    public static bool IsCountrySupported(string countryCode) => GetOrCreateValidator(countryCode) != null;
}
