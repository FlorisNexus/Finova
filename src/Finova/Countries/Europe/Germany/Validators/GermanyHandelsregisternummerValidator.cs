using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Germany.Validators;

/// <summary>
/// Validator for German Register Numbers.
/// Supports HRA, HRB (Commercial), PR (Partnership), GNR (Cooperative), and VR (Association) followed by digits.
/// Format: [Prefix] followed by 1-9 digits (e.g., HRB 12345).
/// </summary>
public partial class GermanyHandelsregisternummerValidator : ITaxIdValidator
{
    [GeneratedRegex(@"^(HRA|HRB|PR|GNR|VR)\s?\d{1,9}$", RegexOptions.IgnoreCase)]
    private static partial Regex FormatRegex();

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespaceRegex();

    public string CountryCode => "DE";

    public ValidationResult Validate(string? instance) => ValidateHandelsregisternummer(instance);

    public string? Parse(string? instance)
    {
        var normalized = Normalize(instance);
        return ValidateHandelsregisternummer(normalized).IsValid ? normalized : null;
    }

    /// <summary>
    /// Validates a German Handelsregisternummer.
    /// </summary>
    public static ValidationResult ValidateHandelsregisternummer(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.HandelsregisternummerCannotBeEmpty);
        }

        // Normalize for validation check (remove spaces to handle "HRB 123" and "HRB123" consistently if needed, 
        // but the regex handles optional space. Let's stick to the regex check on the input or normalized input.)
        // The requirement says "Format validation (Regex: (HRA|HRB)\s?\d+)".

        // Let's trim the input first.
        var trimmed = number.Trim();

        if (!FormatRegex().IsMatch(trimmed))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidGermanyHandelsregisternummerFormat);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Formats a German register number.
    /// Ensures a space between the prefix and the digits (e.g., "HRB 12345").
    /// </summary>
    public static string Format(string? number)
    {
        if (!ValidateHandelsregisternummer(number).IsValid)
        {
            throw new ArgumentException("Invalid register number", nameof(number));
        }

        var normalized = Normalize(number);
        
        // Find the split point between prefix and digits
        int splitPoint = 0;
        while (splitPoint < normalized.Length && !char.IsDigit(normalized[splitPoint]))
        {
            splitPoint++;
        }

        if (splitPoint > 0 && splitPoint < normalized.Length)
        {
            return normalized.Insert(splitPoint, " ");
        }

        return normalized;
    }

    /// <summary>
    /// Normalizes a German Handelsregisternummer.
    /// Removes spaces and converts to uppercase (e.g., "HRB12345").
    /// </summary>
    public static string Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return string.Empty;
        }

        var noSpaces = WhitespaceRegex().Replace(number, "").ToUpperInvariant();
        return noSpaces;
    }
}
