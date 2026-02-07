using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Ireland.Validators;

/// <summary>
/// Validator for Irish Personal Public Service Number (PPSN).
/// Format: 8 or 9 characters.
/// </summary>
public partial class IrelandNationalIdValidator : NationalIdValidatorBase
{
    private static readonly int[] Weights = { 8, 7, 6, 5, 4, 3, 2 };

    /// <inheritdoc/>
        public override string CountryCode => "IE";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 8 || sanitized.Length == 9;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized)
    {
        // First 7 must be digits
        if (!long.TryParse(sanitized.Substring(0, 7), out _))
        {
            return false;
        }

        // 8th char must be a letter
        if (!char.IsLetter(sanitized[7]))
        {
            return false;
        }

        // Optional 9th char must be a letter
        if (sanitized.Length == 9 && !char.IsLetter(sanitized[8]))
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        string digitsPart = sanitized.Substring(0, 7);
        char checkChar = sanitized[7];
        char? lastChar = sanitized.Length == 9 ? sanitized[8] : null;

        int sum = 0;
        for (int i = 0; i < 7; i++)
        {
            sum += (digitsPart[i] - '0') * Weights[i];
        }

        if (lastChar.HasValue)
        {
            sum += 9 * (lastChar.Value - 'A' + 1);
        }

        int remainder = sum % 23;
        char expectedCheckChar = remainder == 0 ? 'W' : (char)('A' + remainder - 1);

        return checkChar == expectedCheckChar
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Static validation method for Irish PPSN.
    /// </summary>
        public static ValidationResult ValidateStatic(string? nationalId) => new IrelandNationalIdValidator().Validate(nationalId);
}