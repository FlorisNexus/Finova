using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Spain.Validators;

/// <summary>
/// Validator for Spanish National ID (DNI) and Foreigner ID (NIE).
/// Format: 9 characters.
/// </summary>
public partial class SpainNationalIdValidator : NationalIdValidatorBase
{
    private const string ControlLetters = "TRWAGMYFPDXBNJZSQVHLCKE";

    /// <inheritdoc/>
        public override string CountryCode => "ES";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 9;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized)
    {
        char firstChar = sanitized[0];
        if (char.IsDigit(firstChar))
        {
            // DNI: 8 digits + 1 letter
            return Regex.IsMatch(sanitized, @"^\d{8}[A-Z]$");
        }
        else if (firstChar == 'X' || firstChar == 'Y' || firstChar == 'Z')
        {
            // NIE: X/Y/Z + 7 digits + 1 letter
            return Regex.IsMatch(sanitized, @"^[XYZ]\d{7}[A-Z]$");
        }
        return false;
    }

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        char firstChar = sanitized[0];
        string numberPart;

        if (char.IsDigit(firstChar))
        {
            numberPart = sanitized.Substring(0, 8);
        }
        else
        {
            string prefix = firstChar == 'X' ? "0" : (firstChar == 'Y' ? "1" : "2");
            numberPart = prefix + sanitized.Substring(1, 7);
        }

        long number = long.Parse(numberPart);
        int index = (int)(number % 23);
        char expectedLetter = ControlLetters[index];

        return sanitized[8] == expectedLetter
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Static validation method for Spanish DNI/NIE.
    /// </summary>
        public static ValidationResult ValidateStatic(string? input) => new SpainNationalIdValidator().Validate(input);
}