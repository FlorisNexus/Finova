using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.MiddleEast.Israel.Validators;

/// <summary>
/// Validator for Israeli VAT Registration Number (Authorized Dealer Number).
/// Format: 9 digits.
/// </summary>
public partial class IsraelVatValidator : VatValidatorBase
{
    private const string CountryCodePrefix = "IL";

    [GeneratedRegex(@"^\d{9}$")]
    private static partial Regex VatRegex();

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new IsraelVatValidator().ValidateInternal(input);


    /// <inheritdoc/>
    protected override ValidationResult ValidateInternal(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var sanitized = VatSanitizer.Sanitize(vat)!;
        var cleaned = sanitized;

        // Remove IL prefix if present
        if (cleaned.StartsWith(CountryCodePrefix, StringComparison.OrdinalIgnoreCase))
        {
            cleaned = cleaned[2..];
        }

        // Pad with leading zeros if necessary
        if (cleaned.Length < 9 && cleaned.Length >= 5)
        {
            cleaned = cleaned.PadLeft(9, '0');
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
    protected override VatDetails CreateDetails(string cleaned)
    {
        return new VatDetails
        {
            CountryCode = CountryCode,
            VatNumber = cleaned,
            IsValid = true,
            IdentifierKind = "Authorized Dealer Number",
            IsEuVat = false,
            Notes = "מספר עוסק מורשה (Authorized Dealer Number)"
        };
    }

    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 9;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        // Israeli ID/VAT uses a modified Luhn algorithm
        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            int digit = cleaned[i] - '0';
            int weight = (i % 2 == 0) ? 1 : 2;
            int product = digit * weight;

            if (product > 9)
            {
                product -= 9;
            }

            sum += product;
        }

        return sum % 10 == 0
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidIsraelVatChecksum);
    }

    /// <summary>
    /// Static validation method for Israeli VAT numbers.
    /// </summary>
    public static ValidationResult ValidateVat(string? vat) => new IsraelVatValidator().ValidateInternal(vat);

    /// <summary>
    /// Gets details for an Israeli VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new IsraelVatValidator().Parse(vat);
}
