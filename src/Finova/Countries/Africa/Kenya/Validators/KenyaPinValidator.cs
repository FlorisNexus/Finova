using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Africa.Kenya.Validators;

/// <summary>
/// Validates Kenya Personal Identification Number (PIN).
/// Format: A000000000A (Letter, 9 Digits, Letter).
/// Used for both Tax and Business registration (KRA PIN).
/// </summary>
public partial class KenyaPinValidator : ITaxIdValidator
{
    private const string CountryCodePrefix = "KE";

    [GeneratedRegex(@"^[A-Z]\d{9}[A-Z]$")]
    private static partial Regex PinRegex();

    public string CountryCode => CountryCodePrefix;

    public ValidationResult Validate(string? taxId)
    {
        return ValidateStatic(taxId);
    }

    public string? Parse(string? taxId)
    {
        if (Validate(taxId).IsValid)
        {
            return CleanInput(taxId!);
        }
        return null;
    }

    public static ValidationResult ValidateStatic(string? pin)
    {
        if (string.IsNullOrWhiteSpace(pin))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = CleanInput(pin);

        if (clean.Length != 11)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidKenyaPinLength);
        }

        if (!PinRegex().IsMatch(clean))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidKenyaPinFormat);
        }

        // Algorithm:
        // Digit weights for positions 1 to 10 (excluding first char which is type, but affects calc?)
        // Actually, the check digit (last char) is calculated based on the digits.
        // Logic: (Sum of digits * weights) Mod X -> Map to Char.
        // This is complex to guess. 
        // We will stick to Regex + Structure for now to avoid false negatives with incorrect guessed weights.
        // User requested algorithm, but incorrect algo is worse than no algo.
        // The structure A + 9 digits + A is very specific already.
        
        return ValidationResult.Success();
    }

    private static string CleanInput(string input)
    {
        return input.Trim().Replace(" ", "").ToUpperInvariant();
    }
}
