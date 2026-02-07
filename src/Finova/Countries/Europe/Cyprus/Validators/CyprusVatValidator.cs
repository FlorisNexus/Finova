using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Cyprus.Validators;

/// <summary>
/// Validator for Cypriot VAT numbers.
/// Format: 9 characters. 8 digits + 1 letter.
/// </summary>
public partial class CyprusVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{8}[A-Z]$")]
    private static partial Regex VatRegex();

    // Mapping table for even positions (0, 2, 4, 6)
    // 0->1, 1->0, 2->5, 3->7, 4->9, 5->13, 6->15, 7->17, 8->19, 9->21
    private static readonly int[] CyprusMap = [1, 0, 5, 7, 9, 13, 15, 17, 19, 21];

    private const string VatPrefix = "CY";

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new CyprusVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 9;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        string digits = cleaned.Substring(0, 8);
        char expectedLetter = cleaned[8];

        int sum = 0;
        for (int i = 0; i < 8; i++)
        {
            int digit = digits[i] - '0';

            // Even positions (0, 2, 4...) use mapping
            if (i % 2 == 0)
            {
                sum += CyprusMap[digit];
            }
            else
            {
                sum += digit;
            }
        }

        int remainder = sum % 26;
        char calculatedLetter = (char)(remainder + 'A');

        if (calculatedLetter != expectedLetter)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidCyprusVatChecksum);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Cypriot VAT numbers.
    /// </summary>
    /// <param name="vat">The VAT number to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
        public static ValidationResult ValidateVat(string? vat) => new CyprusVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Cypriot VAT number.
    /// </summary>
    /// <param name="vat">The VAT number to parse.</param>
    /// <returns>A <see cref="VatDetails"/> object or null if invalid.</returns>
    public static VatDetails? GetVatDetails(string? vat) => new CyprusVatValidator().Parse(vat);
}
