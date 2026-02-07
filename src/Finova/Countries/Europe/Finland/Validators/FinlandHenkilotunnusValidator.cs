using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Finland.Validators;

/// <summary>
/// Validator for Finland Personal Identity Code (Henkilötunnus).
/// Format: DDMMYY{century_sign}NNNC (11 characters).
/// The century sign (+, -, A) is meaningful and must be preserved during sanitization.
/// </summary>
public partial class FinlandHenkilotunnusValidator : NationalIdValidatorBase
{
    private const string CheckChars = "0123456789ABCDEFHJKLMNPRSTUVWXY";

    /// <inheritdoc/>
        public override string CountryCode => "FI";

    /// <summary>
    /// Sanitizes Finnish Henkilötunnus input, preserving the century sign characters (+, -).
    /// Only removes whitespace and converts letters to uppercase.
    /// </summary>
    private static string? SanitizeHetu(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return input;
        }

        // Only trim whitespace and uppercase; preserve +, -, A (century signs)
        return input.Trim().ToUpperInvariant();
    }

    /// <inheritdoc/>
    protected override ValidationResult ValidateInternal(string? nationalId)
    {
        if (string.IsNullOrWhiteSpace(nationalId))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var sanitized = SanitizeHetu(nationalId)!;

        if (!IsValidLength(sanitized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength,
                string.Format(ValidationMessages.InvalidLength, CountryCode));
        }

        if (!ValidateFormat(sanitized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat,
                string.Format(ValidationMessages.InvalidFormat, CountryCode));
        }

        return ValidateChecksum(sanitized);
    }

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 11;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized)
    {
        // Check century sign
        char centurySign = sanitized[6];
        if (centurySign != '+' && centurySign != '-' && centurySign != 'A')
        {
            return false;
        }

        // Extract date and individual number
        string dateAndIndividual = sanitized.Substring(0, 6) + sanitized.Substring(7, 3);

        if (!long.TryParse(dateAndIndividual, out _))
        {
            return false;
        }

        // Validate Date
        int day = int.Parse(sanitized.Substring(0, 2));
        int month = int.Parse(sanitized.Substring(2, 2));

        return month >= 1 && month <= 12 && day >= 1 && day <= 31;
    }

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        string dateAndIndividual = sanitized.Substring(0, 6) + sanitized.Substring(7, 3);
        long number = long.Parse(dateAndIndividual);

        int remainder = (int)(number % 31);
        char expectedCheckChar = CheckChars[remainder];

        return sanitized[10] == expectedCheckChar
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Static validation method for Finnish Henkilötunnus.
    /// </summary>
    public static ValidationResult ValidateStatic(string? nationalId) => new FinlandHenkilotunnusValidator().Validate(nationalId);
}