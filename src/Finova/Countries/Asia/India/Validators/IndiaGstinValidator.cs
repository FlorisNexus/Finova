using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Asia.India.Validators;

/// <summary>
/// Validator for Indian Goods and Services Tax Identification Number (GSTIN).
/// Format: 15 characters.
/// </summary>
public partial class IndiaGstinValidator : VatValidatorBase
{
    private const string CountryCodePrefix = "IN";

    // GSTIN format: 2 digits state + 10 char PAN + 1 entity + 1 'Z' + 1 checksum
    [GeneratedRegex(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z][0-9A-Z]Z[0-9A-Z]$")]
    private static partial Regex GstinRegex();

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new IndiaGstinValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 15;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned)
    {
        if (!GstinRegex().IsMatch(cleaned))
        {
            return false;
        }

        // Validate state code (01-37)
        if (!int.TryParse(cleaned[..2], out int stateCode) || stateCode < 1 || stateCode > 37)
        {
            return false;
        }

        // Extract the embedded PAN and validate its structure
        string embeddedPan = cleaned.Substring(2, 10);
        return ValidatePanStructure(embeddedPan);
    }

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        // Position 14 must be 'Z' (default)
        if (cleaned[13] != 'Z')
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidIndiaGstinPosition14);
        }

        // Validate checksum (position 15)
        char expectedChecksum = CalculateGstinChecksum(cleaned[..14]);
        return cleaned[14] == expectedChecksum
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Validates the structure of a PAN embedded in GSTIN.
    /// </summary>
    private static bool ValidatePanStructure(string pan)
    {
        if (pan.Length != 10)
        {
            return false;
        }

        // First 5 characters: letters
        for (int i = 0; i < 5; i++)
        {
            if (!char.IsLetter(pan[i]))
            {
                return false;
            }
        }

        // Next 4 characters: digits
        for (int i = 5; i < 9; i++)
        {
            if (!char.IsDigit(pan[i]))
            {
                return false;
            }
        }

        // Last character: letter
        if (!char.IsLetter(pan[9]))
        {
            return false;
        }

        // 4th character validation (Status)
        char status = pan[3];
        string validStatuses = "PCHABGJLFT";
        return validStatuses.Contains(status);
    }

    /// <summary>
    /// Calculates the GSTIN checksum character using the modified Luhn algorithm.
    /// </summary>
    private static char CalculateGstinChecksum(string input)
    {
        const string charSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        int factor = 2;
        int sum = 0;
        int mod = charSet.Length;

        for (int i = input.Length - 1; i >= 0; i--)
        {
            int codePoint = charSet.IndexOf(input[i]);
            int addend = factor * codePoint;

            factor = factor == 2 ? 1 : 2;
            addend = (addend / mod) + (addend % mod);
            sum += addend;
        }

        int remainder = sum % mod;
        int checkCodePoint = (mod - remainder) % mod;

        return charSet[checkCodePoint];
    }

    /// <summary>
    /// Static validation method for Indian GSTIN numbers.
    /// </summary>
    public static ValidationResult ValidateVat(string? vat) => new IndiaGstinValidator().Validate(vat);

    /// <summary>
    /// Gets details for an Indian GSTIN number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new IndiaGstinValidator().Parse(vat);
}