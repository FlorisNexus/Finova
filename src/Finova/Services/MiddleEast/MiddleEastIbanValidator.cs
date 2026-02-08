using System.Collections.Concurrent;
using Finova.Core.Common;
using Finova.Core.Iban;
using Finova.Countries.MiddleEast.Bahrain.Validators;
using Finova.Countries.MiddleEast.Iraq.Validators;
using Finova.Countries.MiddleEast.Israel.Validators;
using Finova.Countries.MiddleEast.Jordan.Validators;
using Finova.Countries.MiddleEast.Kuwait.Validators;
using Finova.Countries.MiddleEast.Lebanon.Validators;
using Finova.Countries.MiddleEast.Oman.Validators;
using Finova.Countries.MiddleEast.Palestine.Validators;
using Finova.Countries.MiddleEast.Qatar.Validators;
using Finova.Countries.MiddleEast.SaudiArabia.Validators;
using Finova.Countries.MiddleEast.UAE.Validators;
using Finova.Countries.MiddleEast.Yemen.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Services;

/// <summary>
/// Unified validator for Middle Eastern IBANs.
/// Delegates validation to specific country validators based on the country code prefix.
/// </summary>
public class MiddleEastIbanValidator : IIbanValidator
{
    private static readonly ConcurrentDictionary<string, IIbanValidator> _staticValidators = new();

    private readonly IServiceProvider? _serviceProvider;
    private IEnumerable<IIbanValidator>? _validators;

    public MiddleEastIbanValidator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public MiddleEastIbanValidator()
    {
        _serviceProvider = null;
    }

    private IEnumerable<IIbanValidator> GetValidators()
    {
        if (_validators == null && _serviceProvider != null)
        {
            _validators = _serviceProvider.GetServices<IIbanValidator>()
                                          .Where(v => v.GetType() != typeof(MiddleEastIbanValidator));
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
    /// Validates a Middle Eastern IBAN using static logic.
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

        var validator = _staticValidators.GetOrAdd(country, code => code switch
        {
            "AE" => new UAEIbanValidator(),
            "BH" => new BahrainIbanValidator(),
            "IL" => new IsraelIbanValidator(),
            "IQ" => new IraqIbanValidator(),
            "JO" => new JordanIbanValidator(),
            "KW" => new KuwaitIbanValidator(),
            "LB" => new LebanonIbanValidator(),
            "OM" => new OmanIbanValidator(),
            "PS" => new PalestineIbanValidator(),
            "QA" => new QatarIbanValidator(),
            "SA" => new SaudiArabiaIbanValidator(),
            "YE" => new YemenIbanValidator(),
            _ => null!
        });

        return validator != null
            ? validator.Validate(iban)
            : ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountryOrInvalidIban);
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
}
