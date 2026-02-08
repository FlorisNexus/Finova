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
        // 1. Basic generic validation (structure, mod97)
        if (!IbanHelper.IsValidIban(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidIban, ValidationMessages.InvalidIban);
        }

        var countryCode = IbanHelper.GetCountryCode(iban).ToUpperInvariant();

        // 2. Routing to continent-specific static validators for enhanced rules
        // Europe (Primary coverage)
        var result = EuropeIbanValidator.ValidateIban(iban);
        if (result.IsValid || result.Errors.All(e => e.Code != ValidationErrorCode.UnsupportedCountry))
        {
            return result;
        }

        // Africa
        result = AfricaIbanValidator.ValidateIban(iban);
        if (result.IsValid || result.Errors.All(e => e.Code != ValidationErrorCode.UnsupportedCountry))
        {
            return result;
        }

        // Middle East
        result = MiddleEastIbanValidator.ValidateIban(iban);
        if (result.IsValid || result.Errors.All(e => e.Code != ValidationErrorCode.UnsupportedCountry))
        {
            return result;
        }

        // Americas
        result = AmericasIbanValidator.ValidateIban(iban);
        if (result.IsValid || result.Errors.All(e => e.Code != ValidationErrorCode.UnsupportedCountry))
        {
            return result;
        }

        // Asia
        result = AsiaIbanValidator.ValidateIban(iban);
        if (result.IsValid || result.Errors.All(e => e.Code != ValidationErrorCode.UnsupportedCountry))
        {
            return result;
        }

        // 3. Fallback: If it passed the generic Mod97 check above, we consider it valid (generic IBAN support).
        return ValidationResult.Success();
    }

    public ValidationResult Validate(string? iban)
    {
        // 1. Basic generic validation (structure, mod97)
        if (!IbanHelper.IsValidIban(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidIban, ValidationMessages.InvalidIban);
        }

        // 2. Routing to specific country validator
        var countryCode = IbanHelper.GetCountryCode(iban);

        if (_validators.TryGetValue(countryCode, out var validator))
        {
            return validator.Validate(iban);
        }

        // 3. Fallback: If no specific validator exists, but it passed the generic Mod97 check above,
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
