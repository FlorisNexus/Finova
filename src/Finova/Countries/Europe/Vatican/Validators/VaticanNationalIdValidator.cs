using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Vatican.Validators;

/// <summary>
/// Validator for Vatican National Identity Number.
/// </summary>
public partial class VaticanNationalIdValidator : NationalIdValidatorBase
{
    /// <inheritdoc/>
        public override string CountryCode => "VA";

    /// <inheritdoc/>
    protected override ValidationResult ValidateInternal(string? nationalId)
    {
        if (string.IsNullOrWhiteSpace(nationalId))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.VaticanNationalIdNotSupported);
    }

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => false;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized) => false;

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.VaticanNationalIdNotSupported);
    }

    /// <summary>
    /// Static validation method for Vatican National ID.
    /// </summary>
        public static ValidationResult ValidateStatic(string? nationalId) => new VaticanNationalIdValidator().Validate(nationalId);
}