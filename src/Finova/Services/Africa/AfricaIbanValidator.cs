using System.Collections.Concurrent;
using Finova.Core.Common;
using Finova.Core.Iban;
using Finova.Countries.Africa.Algeria.Validators;
using Finova.Countries.Africa.Angola.Validators;
using Finova.Countries.Africa.Benin.Validators;
using Finova.Countries.Africa.BurkinaFaso.Validators;
using Finova.Countries.Africa.Burundi.Validators;
using Finova.Countries.Africa.Cameroon.Validators;
using Finova.Countries.Africa.CapeVerde.Validators;
using Finova.Countries.Africa.CentralAfricanRepublic.Validators;
using Finova.Countries.Africa.Chad.Validators;
using Finova.Countries.Africa.Comoros.Validators;
using Finova.Countries.Africa.Congo.Validators;
using Finova.Countries.Africa.CoteDIvoire.Validators;
using Finova.Countries.Africa.Djibouti.Validators;
using Finova.Countries.Africa.Egypt.Validators;
using Finova.Countries.Africa.EquatorialGuinea.Validators;
using Finova.Countries.Africa.Gabon.Validators;
using Finova.Countries.Africa.GuineaBissau.Validators;
using Finova.Countries.Africa.Libya.Validators;
using Finova.Countries.Africa.Madagascar.Validators;
using Finova.Countries.Africa.Mali.Validators;
using Finova.Countries.Africa.Mauritania.Validators;
using Finova.Countries.Africa.Morocco.Validators;
using Finova.Countries.Africa.Mozambique.Validators;
using Finova.Countries.Africa.Niger.Validators;
using Finova.Countries.Africa.SaoTomeAndPrincipe.Validators;
using Finova.Countries.Africa.Senegal.Validators;
using Finova.Countries.Africa.Seychelles.Validators;
using Finova.Countries.Africa.Somalia.Validators;
using Finova.Countries.Africa.Sudan.Validators;
using Finova.Countries.Africa.Togo.Validators;
using Finova.Countries.Africa.Tunisia.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Services;

/// <summary>
/// Unified validator for African IBANs.
/// Delegates validation to specific country validators based on the country code prefix.
/// </summary>
public class AfricaIbanValidator : IIbanValidator
{
    private static readonly ConcurrentDictionary<string, IIbanValidator> _staticValidators = new();

    private readonly IServiceProvider? _serviceProvider;
    private IEnumerable<IIbanValidator>? _validators;

    public AfricaIbanValidator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public AfricaIbanValidator()
    {
        _serviceProvider = null;
    }

    private IEnumerable<IIbanValidator> GetValidators()
    {
        if (_validators == null && _serviceProvider != null)
        {
            _validators = _serviceProvider.GetServices<IIbanValidator>()
                                          .Where(v => v.GetType() != typeof(AfricaIbanValidator));
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
    /// Validates an African IBAN using static logic.
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
            "DZ" => new AlgeriaIbanValidator(),
            "AO" => new AngolaIbanValidator(),
            "BJ" => new BeninIbanValidator(),
            "BF" => new BurkinaFasoIbanValidator(),
            "BI" => new BurundiIbanValidator(),
            "CM" => new CameroonIbanValidator(),
            "CV" => new CapeVerdeIbanValidator(),
            "CF" => new CentralAfricanRepublicIbanValidator(),
            "TD" => new ChadIbanValidator(),
            "KM" => new ComorosIbanValidator(),
            "CG" => new CongoIbanValidator(),
            "CI" => new CoteDIvoireIbanValidator(),
            "DJ" => new DjiboutiIbanValidator(),
            "EG" => new EgyptIbanValidator(),
            "GQ" => new EquatorialGuineaIbanValidator(),
            "GA" => new GabonIbanValidator(),
            "GW" => new GuineaBissauIbanValidator(),
            "LY" => new LibyaIbanValidator(),
            "MG" => new MadagascarIbanValidator(),
            "ML" => new MaliIbanValidator(),
            "MR" => new MauritaniaIbanValidator(),
            "MA" => new MoroccoIbanValidator(),
            "MZ" => new MozambiqueIbanValidator(),
            "NE" => new NigerIbanValidator(),
            "ST" => new SaoTomeAndPrincipeIbanValidator(),
            "SN" => new SenegalIbanValidator(),
            "SC" => new SeychellesIbanValidator(),
            "SO" => new SomaliaIbanValidator(),
            "SD" => new SudanIbanValidator(),
            "TG" => new TogoIbanValidator(),
            "TN" => new TunisiaIbanValidator(),
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
