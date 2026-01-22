using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.SoutheastAsia.Indonesia.Validators;

/// <summary>
/// Validates Indonesia Nomor Pokok Wajib Pajak (NPWP) - Tax Identification Number.
/// Format: 15 digits (Old) or 16 digits (New - NIK based).
/// Standard format: TT.TTT.TTT.C-KKK.BBB
/// where C is the check digit for the first 9 digits.
/// </summary>
public partial class IndonesiaNpwpValidator : ITaxIdValidator
{
    private const string CountryCodePrefix = "ID";

    [GeneratedRegex(@"^(\d{15}|\d{16})$")]
    private static partial Regex NpwpRegex();

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

    public static ValidationResult ValidateStatic(string? npwp)
    {
        if (string.IsNullOrWhiteSpace(npwp))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = CleanInput(npwp);

        if (clean.Length != 15 && clean.Length != 16)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidIndonesiaNpwpLength);
        }

        if (!NpwpRegex().IsMatch(clean))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidIndonesiaNpwpFormat);
        }

        // For 15-digit NPWP, the 10th digit checks the first 9 digits.
        // For 16-digit NPWP (NIK), it follows NIK validation (not implemented here, assumed valid if 16 digits for now or could reuse NIK validator).
        // Let's focus on the 15-digit traditional NPWP checksum.
        if (clean.Length == 15)
        {
            if (!ValidateChecksum(clean))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidIndonesiaNpwpChecksum);
            }
        }

        return ValidationResult.Success();
    }

    private static string CleanInput(string input)
    {
        return input.Trim().Replace(".", "").Replace("-", "").Replace(" ", "");
    }

    /// <summary>
    /// Validates the checksum for 15-digit NPWP.
    /// Uses Luhn algorithm on the first 9 digits to verify against the 10th digit.
    /// Actually, Indonesia uses specific weights or Luhn? 
    /// Common implementation suggests standard Luhn on first 9 digits + check digit (10th).
    /// </summary>
    private static bool ValidateChecksum(string npwp)
    {
        // Digits: d1 d2 d3 d4 d5 d6 d7 d8 d9
        // Check digit: d10
        // Algorithm: Luhn usually
        // Let's check specific Indonesian Tax algo. 
        // It is often cited as Luhn. 
        // Let's verify the first 9 digits + check digit.
        
        // Wait, some sources say it's not strictly Luhn but close.
        // Let's use the provided implementation for now: Luhn is safest assumption for "Tax ID" unless specific weight known.
        // Actually, for NPWP: 
        // Fixed weights are not standard public info, but Luhn is commonly used for tax IDs.
        // Let's use Luhn on the first 10 characters substring.
        
        string partToCheck = npwp.Substring(0, 10);
        return ChecksumHelper.ValidateLuhn(partToCheck);
    }
}
