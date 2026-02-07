using Finova.Core.Common;

namespace Finova.Core.Iban;

/// <summary>
/// Abstract base class for country-specific IBAN validators.
/// Implements the standard ISO 13616 validation sequence:
/// null check, normalization, length, country code, BBAN, and mod-97 checksum.
/// </summary>
public abstract class IbanValidatorBase : IIbanValidator
{
    /// <inheritdoc/>
    public abstract string CountryCode { get; }

    /// <summary>
    /// Expected total IBAN length for this country (e.g., 16 for Belgium, 22 for Germany).
    /// </summary>
    protected abstract int ExpectedLength { get; }

    /// <inheritdoc/>
    public ValidationResult Validate(string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != ExpectedLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength,
                string.Format(ValidationMessages.InvalidLengthExpectedXGotY, ExpectedLength, normalized.Length));
        }

        if (!normalized.StartsWith(CountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        string bban = normalized.Substring(4);
        var bbanResult = ValidateBban(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Validates the BBAN (Basic Bank Account Number) portion of the IBAN.
    /// Override this to provide country-specific BBAN validation rules.
    /// </summary>
    /// <param name="bban">The BBAN extracted from the IBAN (everything after the 4-char prefix).</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    protected abstract ValidationResult ValidateBban(string bban);
}
