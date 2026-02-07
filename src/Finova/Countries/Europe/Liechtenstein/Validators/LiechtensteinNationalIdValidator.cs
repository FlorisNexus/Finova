using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Liechtenstein.Validators;

/// <summary>
/// Validator for Liechtenstein Personal Identification Number (PEID).
/// Format: 13 digits starting with 756.
/// </summary>
public partial class LiechtensteinNationalIdValidator : NationalIdValidatorBase
{
    /// <inheritdoc/>
        public override string CountryCode => "LI";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 13;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized)
    {
        return long.TryParse(sanitized, out _) && sanitized.StartsWith("756");
    }

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        // EAN-13 Checksum
        int sum = 0;
        for (int i = 0; i < 12; i++)
        {
            int digit = sanitized[i] - '0';
            sum += (i % 2 == 0) ? digit : digit * 3;
        }

        int remainder = sum % 10;
        int checkDigit = (remainder == 0) ? 0 : 10 - remainder;

        return checkDigit == (sanitized[12] - '0')
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Static validation method for Liechtenstein PEID.
    /// </summary>
        public static ValidationResult ValidateStatic(string? nationalId) => new LiechtensteinNationalIdValidator().Validate(nationalId);
}