using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Oceania.NewZealand.Validators;

/// <summary>
/// Validates New Zealand Business Number (NZBN).
/// Format: 13 digits.
/// Uses Global Location Number (GLN) validation (Modulo 10).
/// </summary>
public class NewZealandNzbnValidator : ITaxIdValidator
{
    public string CountryCode => "NZ";

    public ValidationResult Validate(string? taxId)
    {
        return ValidateStatic(taxId);
    }

    public string? Parse(string? taxId)
    {
        if (Validate(taxId).IsValid)
        {
            return taxId?.Trim().Replace(" ", "").Replace("-", "");
        }
        return null;
    }

    public static ValidationResult ValidateStatic(string? nzbn)
    {
        if (string.IsNullOrWhiteSpace(nzbn))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = nzbn.Trim().Replace(" ", "").Replace("-", "");

        if (clean.Length != 13)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidNewZealandNzbnLength);
        }

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        // NZBN uses standard GS1/GLN Mod 10 algorithm
        if (!ChecksumHelper.ValidateEan13(clean))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidNewZealandNzbnChecksum);
        }

        return ValidationResult.Success();
    }
}
