using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Netherlands.Validators;

/// <summary>
/// Validator for the Dutch National ID (Burgerservicenummer - BSN).
/// Format: 8 or 9 digits.
/// </summary>
public partial class NetherlandsNationalIdValidator : NationalIdValidatorBase
{
    /// <inheritdoc/>
        public override string CountryCode => "NL";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length >= 8 && sanitized.Length <= 9;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized) => long.TryParse(sanitized, out _);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        string padded = sanitized.PadLeft(9, '0');

        // 11-test (Elfproef)
        // Sum = d1*9 + d2*8 + ... + d8*2 + d9*-1
        int sum = 0;
        for (int i = 0; i < 8; i++)
        {
            sum += (padded[i] - '0') * (9 - i);
        }

        sum += (padded[8] - '0') * -1;

        return sum % 11 == 0
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Static validation method for Dutch BSN.
    /// </summary>
        public static ValidationResult ValidateStatic(string? bsn) => new NetherlandsNationalIdValidator().Validate(bsn);
}