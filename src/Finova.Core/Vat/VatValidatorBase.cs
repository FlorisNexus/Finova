using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Core.Vat;

/// <summary>
/// Abstract base class for country-specific VAT validators.
/// Provides a standard validation flow: null check, sanitization, prefix handling, format check, and checksum validation.
/// </summary>
public abstract class VatValidatorBase : IVatValidator, ITaxIdValidator
{
    /// <inheritdoc/>
    public abstract string CountryCode { get; }

    /// <summary>
    /// Gets a value indicating whether the country code prefix is required in the input.
    /// Default is false (prefix is optional).
    /// </summary>
    protected virtual bool RequireCountryCodePrefix => false;

    /// <summary>
    /// Internal validation logic that can be overridden by derived classes.
    /// </summary>
    /// <param name="vat">The VAT number to validate.</param>
    /// <returns>A <see cref="ValidationResult"/>.</returns>
    protected virtual ValidationResult ValidateInternal(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // 1. Sanitize (Remove dots, spaces, dashes and convert to uppercase)
        var sanitized = VatSanitizer.Sanitize(vat)!;

        // 2. Handle Country Prefix
        var cleaned = sanitized;
        if (cleaned.StartsWith(CountryCode, StringComparison.OrdinalIgnoreCase))
        {
            cleaned = cleaned[CountryCode.Length..];
        }
        else if (RequireCountryCodePrefix)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        // 3. Length Check
        if (!IsValidLength(cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength,
                string.Format(ValidationMessages.InvalidLength, CountryCode));
        }

        // 4. Format/Regex Check
        if (!ValidateFormat(cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat,
                string.Format(ValidationMessages.InvalidVatFormat, CountryCode));
        }

        // 5. Checksum Check
        return ValidateChecksum(cleaned);
    }

    /// <summary>
    /// Checks if the cleaned VAT number (without prefix) has a valid length.
    /// </summary>
    /// <param name="cleaned">The VAT number without country prefix and non-alphanumeric characters.</param>
    /// <returns>True if the length is valid; otherwise, false.</returns>
    protected abstract bool IsValidLength(string cleaned);

    /// <summary>
    /// Validates the format of the cleaned VAT number (e.g., using Regex).
    /// </summary>
    /// <param name="cleaned">The VAT number without country prefix and non-alphanumeric characters.</param>
    /// <returns>True if the format is valid; otherwise, false.</returns>
    protected abstract bool ValidateFormat(string cleaned);

    /// <summary>
    /// Validates the checksum of the cleaned VAT number.
    /// </summary>
    /// <param name="cleaned">The VAT number without country prefix and non-alphanumeric characters.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    protected abstract ValidationResult ValidateChecksum(string cleaned);

    /// <inheritdoc/>
    public virtual VatDetails? Parse(string? vat)
    {
        var result = ValidateInternal(vat);
        if (!result.IsValid)
        {
            return null;
        }

        var sanitized = VatSanitizer.Sanitize(vat)!;
        var cleaned = sanitized;
        if (cleaned.StartsWith(CountryCode, StringComparison.OrdinalIgnoreCase))
        {
            cleaned = cleaned[CountryCode.Length..];
        }

        return CreateDetails(cleaned);
    }

    /// <summary>
    /// Creates the <see cref="VatDetails"/> object for a valid VAT number.
    /// </summary>
    /// <param name="cleaned">The validated VAT number without prefix.</param>
    /// <returns>A populated <see cref="VatDetails"/> object.</returns>
    protected virtual VatDetails CreateDetails(string cleaned)
    {
        return new VatDetails
        {
            CountryCode = CountryCode,
            VatNumber = cleaned,
            IsValid = true
        };
    }

    /// <inheritdoc/>
    public ValidationResult Validate(string? input) => ValidateInternal(input);

    string? IValidator<string>.Parse(string? instance) => Parse(instance)?.VatNumber;
}