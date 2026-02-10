using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.Egypt.Validators;

/// <summary>
/// Validator for Egypt IBANs.
/// Egypt IBAN format: EG + 2 check digits + 4 digits (bank code) + 4 digits (branch) + 17 digits (account)
/// Length: 29 characters
/// Example: EG380019000500000000263180002
/// </summary>
public class EgyptIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "EG";

    /// <inheritdoc/>
    protected override int ExpectedLength => 29;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => bban.All(char.IsDigit)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
}
