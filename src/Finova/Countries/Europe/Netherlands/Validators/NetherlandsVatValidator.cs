using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Netherlands.Validators;

/// <summary>
/// Validator for Dutch VAT numbers (BTW-identificatienummer).
/// Format: 12 characters. 9 digits + 'B' + 2 digits.
/// </summary>
public partial class NetherlandsVatValidator : VatValidatorBase, ITaxIdValidator
{
    [GeneratedRegex(@"^\d{9}B\d{2}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "NL";

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new NetherlandsVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 12;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        // 1. Mod 97 (New format / Standard)
        // Convert letters to numbers (A=10, B=11...)
        System.Text.StringBuilder sb = new();
        foreach (char c in cleaned)
        {
            if (char.IsDigit(c))
            {
                sb.Append(c);
            }
            else
            {
                sb.Append(c - 'A' + 10);
            }
        }
        string mod97Str = sb.ToString();
        bool isMod97Valid = ChecksumHelper.CalculateModulo97(mod97Str) == 1;

        // 2. Elfproef (Old format / RSIN)
        // Weighted Mod 11 on first 9 digits.
        bool isElfproefValid = false;
        if (cleaned.Length >= 9)
        {
            string rsin = cleaned[..9];
            if (long.TryParse(rsin, out _))
            {
                // Weights: 9, 8, 7, 6, 5, 4, 3, 2, -1
                int[] elfWeights = [9, 8, 7, 6, 5, 4, 3, 2, -1];
                if (ChecksumHelper.CalculateWeightedModulo11(rsin, elfWeights) == 0)
                {
                    isElfproefValid = true;
                }
            }
        }

        if (!isMod97Valid && !isElfproefValid)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidNetherlandsVatChecksumMod97OrElfproef);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Dutch VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new NetherlandsVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Dutch VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new NetherlandsVatValidator().Parse(vat);

    /// <summary>
    /// Normalizes a Dutch VAT number.
    /// </summary>
    public static string Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return string.Empty;
        }

        var sanitized = VatSanitizer.Sanitize(number)!;
        if (sanitized.StartsWith(VatPrefix))
        {
            return sanitized[2..];
        }
        return sanitized;
    }

    ValidationResult IValidator<string>.Validate(string? number) => Validate(number);
    string? IValidator<string>.Parse(string? number) => Normalize(number);
}
