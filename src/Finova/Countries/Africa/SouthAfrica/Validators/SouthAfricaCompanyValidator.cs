using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Africa.SouthAfrica.Validators;

/// <summary>
/// Validates South African Companies and Intellectual Property Commission (CIPC) registration numbers.
/// Format: YYYY/NNNNNN/NN (Standard) or YYYYNNNNNNNN (Compact).
/// </summary>
public partial class SouthAfricaCompanyValidator : ITaxIdValidator
{
    private const string CountryCodePrefix = "ZA";

    [GeneratedRegex(@"^\d{4}/?\d{6}/?\d{2}$")]
    private static partial Regex CompanyRegex();

    /// <inheritdoc/>
    public string CountryCode => CountryCodePrefix;

    /// <inheritdoc/>
    public ValidationResult Validate(string? taxId)
    {
        return ValidateStatic(taxId);
    }

    /// <inheritdoc/>
    public string? Parse(string? taxId)
    {
        if (Validate(taxId).IsValid)
        {
            return CleanNumber(taxId!);
        }
        return null;
    }

    /// <summary>
    /// Validates a South African Company Registration Number.
    /// </summary>
    public static ValidationResult ValidateStatic(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = CleanNumber(number);

        if (clean.Length != 12)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidSouthAfricaCompanyNumberLength);
        }

        if (!long.TryParse(clean, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSouthAfricaCompanyNumberFormat);
        }

        // Validate Year (First 4 digits)
        int year = int.Parse(clean.Substring(0, 4));
        if (year < 1800 || year > DateTime.Now.Year + 1)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid registration year.");
        }

        // Luhn check is often cited for CIPC numbers, but officially it's a straight registration number. 
        // We will perform basic structure validation here.
        // If a specific checksum algorithm is required, it can be added. 
        // For now, structure validation is the primary check.
        
        return ValidationResult.Success();
    }

    private static string CleanNumber(string number)
    {
        return number.Trim().Replace("/", "").Replace(" ", "");
    }
}
