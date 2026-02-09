using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.NorthAmerica.UnitedStates.Validators;

/// <summary>
/// Validates United States Social Security Numbers (SSN).
/// </summary>
public class UnitedStatesSsnValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "US";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        return ValidateStatic(input);
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        if (Validate(input).IsValid)
        {
            return input?.Replace("-", "").Replace(" ", "");
        }
        return null;
    }

    /// <summary>
    /// Validates a United States Social Security Number (SSN) (Static).
    /// </summary>
    /// <param name="ssn">The SSN string (e.g., "000-00-0000").</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? ssn)
    {
        return ValidateSsn(ssn);
    }

    /// <summary>
    /// Validates a United States Social Security Number (SSN).
    /// </summary>
    /// <param name="ssn">The SSN string (e.g., "000-00-0000").</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateSsn(string? ssn)
    {
        if (string.IsNullOrWhiteSpace(ssn))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // Remove separators
        var clean = ssn.Replace("-", "").Replace(" ", "");

        if (clean.Length != 9)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLengthExpectedX.Replace("{0}", "9"));
        }

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        // Structure: AAA-GG-SSSS
        // Area Number (AAA): 000 is invalid.
        // Group Number (GG): 00 is invalid.
        // Serial Number (SSSS): 0000 is invalid.
        // Note: AAA = 666 and 900-999 were also invalid, but SSA has randomized assignment since 2011.
        // However, 000, 666 and 900-999 are still considered never issued legacy wise, but for broad validation:
        // Area 000 is invalid. Group 00 is invalid. Serial 0000 is invalid. 
        // 666 is never issued. 900-999 are arguably still not issued.
        // Finova policy: strict on known invalid ranges.

        var area = int.Parse(clean.Substring(0, 3));
        var group = int.Parse(clean.Substring(3, 2));
        var serial = int.Parse(clean.Substring(5, 4));

        if (area == 0 || area == 666 || area >= 900)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSsnArea);
        }

        if (group == 0)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSsnGroup);
        }

        if (serial == 0)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSsnSerial);
        }

        return ValidationResult.Success();
    }
}
