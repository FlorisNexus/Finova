using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.MiddleEast.Oman.Validators;

/// <summary>
/// Validator for Omani VAT numbers.
/// Format: 15 digits. Usually starts with OM.
/// </summary>
public partial class OmanVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{15}$")]
    private static partial Regex VatRegex();

    private const string CountryCodePrefix = "OM";

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new OmanVatValidator().ValidateInternal(input);


    /// <inheritdoc/>
    protected override ValidationResult ValidateInternal(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var sanitized = VatSanitizer.Sanitize(vat)!;
        var cleaned = sanitized;

        // Oman VAT numbers often include the OM prefix in the 15-digit total or as a separate prefix
        if (cleaned.StartsWith(CountryCodePrefix, StringComparison.OrdinalIgnoreCase))
        {
            cleaned = cleaned[2..];
        }

        // If it was already 15 digits including OM, it's now 13.
        // The spec usually says 15 digits total.
        if (cleaned.Length != 15 && sanitized.Length != 15)
        {
             return ValidationResult.Failure(ValidationErrorCode.InvalidLength,
                ValidationMessages.InvalidOmanVatLength);
        }

        // Standardize to 15 digits for format check
        var toCheck = sanitized.StartsWith(CountryCodePrefix) ? sanitized[2..] : sanitized;
        if (toCheck.Length != 15 && toCheck.Length != 13)
        {
             return ValidationResult.Failure(ValidationErrorCode.InvalidLength,
                ValidationMessages.InvalidOmanVatLength);
        }

        if (!toCheck.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    protected override VatDetails CreateDetails(string cleaned)
    {
        return new VatDetails
        {
            CountryCode = CountryCode,
            VatNumber = cleaned,
            IsValid = true,
            IdentifierKind = "VAT"
        };
    }

    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 15 || cleaned.Length == 13;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => cleaned.All(char.IsDigit);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        // No public checksum algorithm available for OM.
        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Omani VAT numbers.
    /// </summary>
    public static ValidationResult ValidateVat(string? vat) => new OmanVatValidator().ValidateInternal(vat);

    /// <summary>
    /// Gets details for an Omani VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new OmanVatValidator().Parse(vat);
}