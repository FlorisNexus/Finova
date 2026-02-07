using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.Sudan.Validators;

/// <summary>
/// Validator for Sudan IBANs.
/// Sudan IBAN format: SD + 2 check digits + 14 digits BBAN.
/// Length: 18 characters.
/// </summary>
public class SudanIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "SD";

    /// <inheritdoc/>
    protected override int ExpectedLength => 18;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => SudanBbanValidator.Validate(bban);
}
