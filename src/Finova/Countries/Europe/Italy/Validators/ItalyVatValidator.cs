using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;
using Finova.Countries.Europe.Italy.Models;

namespace Finova.Countries.Europe.Italy.Validators;

/// <summary>
/// Validator for Italian VAT numbers (Partita IVA).
/// Format: 11 digits.
/// </summary>
public partial class ItalyVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{11}$")]
    private static partial Regex VatRegex();

    private const string CountryCodePrefix = "IT";

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new ItalyVatValidator().ValidateInternal(input);


    /// <inheritdoc/>
    protected override ValidationResult ValidateInternal(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var sanitized = VatSanitizer.Sanitize(vat)!;
        var cleaned = sanitized;

        if (cleaned.StartsWith(CountryCodePrefix, StringComparison.OrdinalIgnoreCase))
        {
            cleaned = cleaned[2..];
        }

        if (!IsValidLength(cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength,
                string.Format(ValidationMessages.InvalidLength, CountryCode));
        }

        if (!ValidateFormat(cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat,
                string.Format(ValidationMessages.InvalidVatFormat, CountryCode));
        }

        return ValidateChecksum(cleaned);
    }

    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 11;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        if (cleaned == "00000000000")
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, string.Format(ValidationMessages.InvalidVatChecksum, "Italy"));
        }

        return ChecksumHelper.ValidateLuhn(cleaned)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, string.Format(ValidationMessages.InvalidVatChecksum, "Italy"));
    }

    /// <inheritdoc/>
    protected override VatDetails CreateDetails(string cleaned)
    {
        var officeCode = cleaned.Substring(7, 3);
        return new ItalyVatDetails
        {
            VatNumber = $"{CountryCodePrefix}{cleaned}",
            CountryCode = CountryCodePrefix,
            IsValid = true,
            OfficeCode = officeCode
        };
    }

    /// <summary>
    /// Static validation method for Italian VAT numbers.
    /// </summary>
    public static ValidationResult ValidateVat(string? vat) => new ItalyVatValidator().ValidateInternal(vat);

    /// <summary>
    /// Gets details for an Italian VAT number.
    /// </summary>
    public static ItalyVatDetails? GetVatDetails(string? vat)
    {
        var validator = new ItalyVatValidator();
        return (ItalyVatDetails?)validator.Parse(vat);
    }
}
