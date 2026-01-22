using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;
using Finova.Core.Vat;

namespace Finova.Countries.SoutheastAsia.Philippines.Validators;

/// <summary>
/// Validates Philippines Tax Identification Number (TIN).
/// Format: 9 digits or 12 digits (with branch code).
/// Example: 123-456-789-000
/// </summary>
public partial class PhilippinesTinValidator : ITaxIdValidator, IVatValidator
{
    private const string CountryCodePrefix = "PH";

    [GeneratedRegex(@"^(\d{9}|\d{12})$")]
    private static partial Regex TinRegex();

    public string CountryCode => CountryCodePrefix;

    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    public ValidationResult Validate(string? taxId)
    {
        return ValidateStatic(taxId);
    }

    string? IValidator<string>.Parse(string? taxId)
    {
        if (Validate(taxId).IsValid)
        {
            return CleanInput(taxId!);
        }
        return null;
    }

    public VatDetails? Parse(string? vat)
    {
        if (!Validate(vat).IsValid) return null;
        var clean = CleanInput(vat!);
        return new VatDetails
        {
            VatNumber = clean,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "TIN",
            IsEuVat = false,
            IsViesEligible = false
        };
    }

    public static ValidationResult ValidateStatic(string? tin)
    {
        if (string.IsNullOrWhiteSpace(tin))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = CleanInput(tin);

        if (clean.Length != 9 && clean.Length != 12)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Invalid length for Philippines TIN (expected 9 or 12 digits).");
        }

        if (!TinRegex().IsMatch(clean))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Philippines TIN format.");
        }

        // Checksum: The algorithm is proprietary/complex (Mod 11 variants often used). 
        // For now, we enforce structure strictly.
        
        return ValidationResult.Success();
    }

    private static string CleanInput(string input)
    {
        return input.Trim().Replace("-", "").Replace(" ", "");
    }
}
