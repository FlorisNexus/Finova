using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.Cameroon.Validators;

/// <summary>
/// Validator for Cameroon IBANs.
/// Cameroon IBAN format: CM + 2 check digits + 23 digits BBAN.
/// Length: 27 characters.
/// </summary>
public class CameroonIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "CM";

    /// <inheritdoc/>
    protected override int ExpectedLength => 27;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => CameroonBbanValidator.Validate(bban);
}
