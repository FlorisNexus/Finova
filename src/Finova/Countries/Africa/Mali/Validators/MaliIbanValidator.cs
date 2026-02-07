using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.Mali.Validators;

/// <summary>
/// Validator for Mali IBANs.
/// Mali IBAN format: ML + 2 check digits + 24 characters (1 letter + 23 digits) BBAN.
/// Length: 28 characters.
/// </summary>
public class MaliIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "ML";

    /// <inheritdoc/>
    protected override int ExpectedLength => 28;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => MaliBbanValidator.Validate(bban);
}
