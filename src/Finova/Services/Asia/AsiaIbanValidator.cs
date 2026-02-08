using System.Collections.Concurrent;
using Finova.Core.Common;
using Finova.Core.Iban;
using Finova.Countries.Asia.Kazakhstan.Validators;
using Finova.Countries.Asia.Mongolia.Validators;
using Finova.Countries.Asia.Pakistan.Validators;
using Finova.Countries.Asia.TimorLeste.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Services;

/// <summary>
/// Unified validator for Asian IBANs.
/// Delegates validation to specific country validators based on the country code prefix.
/// </summary>
public class AsiaIbanValidator : IIbanValidator
{
    private static readonly ConcurrentDictionary<string, IIbanValidator> _staticValidators = new();

    private readonly IServiceProvider? _serviceProvider;
    private IEnumerable<IIbanValidator>? _validators;

    public AsiaIbanValidator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public AsiaIbanValidator()
    {
        _serviceProvider = null;
    }

    private IEnumerable<IIbanValidator> GetValidators()
    {
        if (_validators == null && _serviceProvider != null)
        {
            _validators = _serviceProvider.GetServices<IIbanValidator>()
                                          .Where(v => v.GetType() != typeof(AsiaIbanValidator));
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
    /// Validates an Asian IBAN using static logic.
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
            "KZ" => new KazakhstanIbanValidator(),
            "MN" => new MongoliaIbanValidator(),
            "PK" => new PakistanIbanValidator(),
            "TL" => new TimorLesteIbanValidator(),
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
