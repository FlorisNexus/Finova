using Finova.Core.Common;
using Finova.Core.Iban;
using Finova.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Services.Global;

/// <summary>
/// Master global validator for IBANs.
/// Aggregates all registered IIbanValidator instances and routes to the appropriate one based on country code.
/// Replaces the default IbanService in the DI container for full-featured environments.
/// </summary>
public class GlobalIbanValidator : IIbanService, IIbanValidator
{
    private readonly Dictionary<string, IIbanValidator> _validators;

    public GlobalIbanValidator(IEnumerable<IIbanValidator> validators)
    {
        // Index validators by country code, ignoring those without a country code (like other composite validators)
        // and ignoring itself to prevent recursion (though it shouldn't send itself if registered correctly).
        _validators = validators
            .Where(v => !string.IsNullOrEmpty(v.CountryCode) && v.GetType() != typeof(GlobalIbanValidator))
            .ToDictionary(v => v.CountryCode, v => v, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Returns empty string as this is a global validator not tied to a specific country.
    /// </summary>
    public string CountryCode => "";

    /// <summary>
    /// Validates an IBAN using the global static logic.
    /// </summary>
    public static ValidationResult ValidateIban(string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        // Check format: 2 letters (country) + 2 digits (check) + alphanumeric
        // We do this check globally because these rules apply to ALL IBANs regardless of country.
        if (normalized.Length >= 2 && (!char.IsLetter(normalized[0]) || !char.IsLetter(normalized[1])))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidIbanCountryCode);
        }

        if (normalized.Length >= 4 && (!char.IsDigit(normalized[2]) || !char.IsDigit(normalized[3])))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidCheckDigit);
        }

        foreach (char c in normalized)
        {
            if (!char.IsLetterOrDigit(c))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidIbanFormatAlphanumeric);
            }
        }

        var countryCode = IbanHelper.GetCountryCode(iban).ToUpperInvariant();

        // 1. Routing to continent-specific static validators (Primary)
        // This ensures country-specific rules (like exact length) take precedence over generic rules.

        // Europe (Primary coverage)
        if (EuropeIbanValidator.IsCountrySupported(countryCode))
        {
            return EuropeIbanValidator.ValidateIban(iban);
        }

        // Africa
        if (AfricaIbanValidator.IsCountrySupported(countryCode))
        {
            return AfricaIbanValidator.ValidateIban(iban);
        }

        // Middle East
        if (MiddleEastIbanValidator.IsCountrySupported(countryCode))
        {
            return MiddleEastIbanValidator.ValidateIban(iban);
        }

        // Americas
        if (AmericasIbanValidator.IsCountrySupported(countryCode))
        {
            return AmericasIbanValidator.ValidateIban(iban);
        }

        // Asia
        if (AsiaIbanValidator.IsCountrySupported(countryCode))
        {
            return AsiaIbanValidator.ValidateIban(iban);
        }

        // 2. Fallback: Basic generic validation (structure, generic length 15-34, mod97)
        if (normalized.Length < IbanHelper.MinIbanLength || normalized.Length > IbanHelper.MaxIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidIbanLength, $"{IbanHelper.MinIbanLength}-{IbanHelper.MaxIbanLength}", normalized.Length));
        }

        // Validate checksum if no specific validator found
        if (!IbanHelper.ValidateChecksum(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        // 3. Fallback: If it passed the generic Mod97 check above, we consider it valid (generic IBAN support).
        return ValidationResult.Success();
    }

    public ValidationResult Validate(string? iban)
    {
        // 1. Basic generic validation (structure, mod97)
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        // Check format: 2 letters (country) + 2 digits (check) + alphanumeric
        if (normalized.Length >= 2 && (!char.IsLetter(normalized[0]) || !char.IsLetter(normalized[1])))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidIbanCountryCode);
        }

        if (normalized.Length >= 4 && (!char.IsDigit(normalized[2]) || !char.IsDigit(normalized[3])))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidCheckDigit);
        }

        foreach (char c in normalized)
        {
            if (!char.IsLetterOrDigit(c))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidIbanFormatAlphanumeric);
            }
        }

        var countryCode = IbanHelper.GetCountryCode(iban).ToUpperInvariant();

        // 2. Routing to specific country validator
        if (_validators.TryGetValue(countryCode, out var validator))
        {
            return validator.Validate(iban);
        }

        // 3. Fallback: Basic generic validation (generic length 15-34, mod97)
        if (normalized.Length < IbanHelper.MinIbanLength || normalized.Length > IbanHelper.MaxIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidIbanLength, $"{IbanHelper.MinIbanLength}-{IbanHelper.MaxIbanLength}", normalized.Length));
        }

        // Validate checksum if no specific validator found (or if specific validator logic allows fallback, but here we return result of validator)
        if (!IbanHelper.ValidateChecksum(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        // 4. Fallback: If no specific validator exists, but it passed the generic Mod97 check above,
        // we consider it valid (generic IBAN support).
        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    public string FormatIban(string? iban)
    {
        return IbanHelper.FormatIban(iban);
    }

    /// <inheritdoc/>
    public string NormalizeIban(string? iban)
    {
        return IbanHelper.NormalizeIban(iban);
    }

    /// <inheritdoc/>
    public string GetCountryCode(string? iban)
    {
        return IbanHelper.GetCountryCode(iban);
    }

    /// <inheritdoc/>
    public int GetCheckDigits(string? iban)
    {
        return IbanHelper.GetCheckDigits(iban);
    }

    /// <inheritdoc/>
    public bool ValidateChecksum(string? iban)
    {
        return IbanHelper.ValidateChecksum(iban);
    }
}
