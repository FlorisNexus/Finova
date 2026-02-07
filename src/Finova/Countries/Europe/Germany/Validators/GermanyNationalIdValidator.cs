using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Germany.Validators;

/// <summary>
/// Validator for German Identity Card Number (Personalausweisnummer).
/// Format: 9 alphanumeric characters.
/// </summary>
public partial class GermanyNationalIdValidator : NationalIdValidatorBase
{
    /// <inheritdoc/>
        public override string CountryCode => "DE";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 9;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized)
    {
        foreach (char c in sanitized)
        {
            if (!char.IsLetterOrDigit(c))
            {
                return false;
            }
        }
        return true;
    }

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        // Weights: 7, 3, 1 repeating
        int[] weights = { 7, 3, 1 };
        int sum = 0;

        // Calculate sum for the first 8 characters
        for (int i = 0; i < 8; i++)
        {
            int value = GetCharValue(sanitized[i]);
            int weight = weights[i % 3];
            sum += value * weight;
        }

        int calculatedCheckDigit = sum % 10;
        int actualCheckDigit = GetCharValue(sanitized[8]);

        return calculatedCheckDigit == actualCheckDigit
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    private static int GetCharValue(char c)
    {
        if (char.IsDigit(c))
        {
            return c - '0';
        }
        if (char.IsLetter(c))
        {
            return char.ToUpperInvariant(c) - 'A' + 10;
        }
        return 0;
    }

    /// <summary>
    /// Static validation method for German National ID.
    /// </summary>
        public static ValidationResult ValidateStatic(string? input) => new GermanyNationalIdValidator().Validate(input);
}