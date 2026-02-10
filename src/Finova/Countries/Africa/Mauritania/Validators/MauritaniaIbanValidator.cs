using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.Mauritania.Validators;

/// <summary>
/// Validator for Mauritania IBANs.
/// Mauritania IBAN format: MR + 2 check digits + 5 digits (bank code) + 5 digits (branch) + 11 digits (account) + 2 digits (key)
/// Length: 27 characters
/// Example: MR1300020001010000123456753
/// </summary>
public class MauritaniaIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "MR";

    /// <inheritdoc/>
    protected override int ExpectedLength => 27;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => bban.All(char.IsDigit)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
}
