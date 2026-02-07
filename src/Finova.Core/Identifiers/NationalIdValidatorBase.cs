using Finova.Core.Common;

namespace Finova.Core.Identifiers;

/// <summary>
/// Abstract base class for country-specific National ID validators.
/// Provides a standard validation flow: null check, sanitization, length check, format check, and checksum validation.
/// </summary>
public abstract class NationalIdValidatorBase : INationalIdValidator
{
    /// <inheritdoc/>
    public abstract string CountryCode { get; }

    /// <summary>
    /// Internal validation logic that can be overridden by derived classes.
    /// </summary>
    /// <param name="nationalId">The National ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    protected virtual ValidationResult ValidateInternal(string? nationalId)
    {
        if (string.IsNullOrWhiteSpace(nationalId))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // 1. Sanitize (Remove dots, spaces, dashes and convert to uppercase)
        var sanitized = InputSanitizer.Sanitize(nationalId)!;

        // 2. Length Check
        if (!IsValidLength(sanitized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength,
                string.Format(ValidationMessages.InvalidLength, CountryCode));
        }

        // 3. Format/Regex Check
        if (!ValidateFormat(sanitized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat,
                string.Format(ValidationMessages.InvalidFormat, CountryCode));
        }

        // 4. Checksum Check
        return ValidateChecksum(sanitized);
    }

    /// <summary>
    /// Checks if the sanitized National ID has a valid length.
    /// </summary>
    /// <param name="sanitized">The National ID after non-alphanumeric character removal.</param>
    /// <returns>True if the length is valid; otherwise, false.</returns>
    protected abstract bool IsValidLength(string sanitized);

    /// <summary>
    /// Validates the format of the sanitized National ID (e.g., using Regex).
    /// </summary>
    /// <param name="sanitized">The National ID after non-alphanumeric character removal.</param>
    /// <returns>True if the format is valid; otherwise, false.</returns>
    protected abstract bool ValidateFormat(string sanitized);

    /// <summary>
    /// Validates the checksum of the sanitized National ID.
    /// </summary>
    /// <param name="sanitized">The National ID after non-alphanumeric character removal.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    protected abstract ValidationResult ValidateChecksum(string sanitized);

    /// <inheritdoc/>
    public virtual string? Parse(string? nationalId)
    {
        var result = ValidateInternal(nationalId);
        if (!result.IsValid)
        {
            return null;
        }

        return InputSanitizer.Sanitize(nationalId);
    }

    /// <inheritdoc/>
    public ValidationResult Validate(string? input) => ValidateInternal(input);
}
