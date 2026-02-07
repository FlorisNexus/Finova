using System.Numerics;
using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.France.Validators;

/// <summary>
/// Validator for French National ID (Numéro de Sécurité Sociale / NIR).
/// Format: 15 characters (13 digits/chars + 2 key digits).
/// </summary>
public partial class FranceNationalIdValidator : NationalIdValidatorBase
{
    /// <inheritdoc/>
        public override string CountryCode => "FR";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 15;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized)
    {
        // Regex: First digit (1/2), then 12 digits OR specific Corsica pattern, then 2 digits key
        return Regex.IsMatch(sanitized, @"^[12]\d{14}$") || 
               Regex.IsMatch(sanitized, @"^[12]\d{4}(2A|2B)\d{6}\d{2}$");
    }

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        string numberPart = sanitized.Substring(0, 13);
        string keyPart = sanitized.Substring(13, 2);

        if (!long.TryParse(keyPart, out long key))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFranceNationalIdKeyFormat);
        }

        // Handle Corsica (2A -> 19, 2B -> 18) for calculation
        string numericString = numberPart;
        if (numberPart.Contains("2A"))
        {
            numericString = numberPart.Replace("2A", "19");
        }
        else if (numberPart.Contains("2B"))
        {
            numericString = numberPart.Replace("2B", "18");
        }

        if (!BigInteger.TryParse(numericString, out BigInteger number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFranceNationalIdNumeric);
        }

        // NIR Key Formula: 97 - (Number % 97)
        long calculatedKey = 97 - (long)(number % 97);

        return calculatedKey == key
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidFranceNationalIdChecksum);
    }

    /// <summary>
    /// Static validation method for French National ID.
    /// </summary>
        public static ValidationResult ValidateStatic(string? input) => new FranceNationalIdValidator().Validate(input);
}