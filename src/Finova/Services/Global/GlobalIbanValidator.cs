using Finova.Core.Common;
using Finova.Core.Iban;

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

    private static ValidationResult? PreValidate(string? iban, out string normalized, out string countryCode)
    {
        normalized = "";
        countryCode = "";

        if (string.IsNullOrWhiteSpace(iban))
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);

        normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length >= 2 && (!char.IsLetter(normalized[0]) || !char.IsLetter(normalized[1])))
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidIbanCountryCode);

        if (normalized.Length >= 4 && (!char.IsDigit(normalized[2]) || !char.IsDigit(normalized[3])))
            return ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidCheckDigit);

        foreach (char c in normalized)
        {
            if (!char.IsLetterOrDigit(c))
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidIbanFormatAlphanumeric);
        }

        countryCode = IbanHelper.GetCountryCode(iban).ToUpperInvariant();
        return null;
    }

    /// <summary>
    /// Validates an IBAN using the global static logic.
    /// </summary>
    public static ValidationResult ValidateIban(string? iban)
    {
        var error = PreValidate(iban, out var normalized, out var countryCode);
        if (error != null) return error;

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
        var error = PreValidate(iban, out var normalized, out var countryCode);
        if (error != null) return error;

        // Routing to specific country validator
        if (_validators.TryGetValue(countryCode, out var validator))
        {
            return validator.Validate(iban);
        }

        // 3. Fallback: Basic generic validation (generic length 15-34, mod97)
        if (normalized.Length < IbanHelper.MinIbanLength || normalized.Length > IbanHelper.MaxIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidIbanLength, $"{IbanHelper.MinIbanLength}-{IbanHelper.MaxIbanLength}", normalized.Length));
        }

        // If no country found, display error unsupported country
        return ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountry);
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
