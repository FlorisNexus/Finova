using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.Madagascar.Validators;

/// <summary>
/// Validator for Madagascar IBANs.
/// Madagascar IBAN format: MG + 2 check digits + 23 digits BBAN.
/// Length: 27 characters.
/// </summary>
public class MadagascarIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "MG";

    /// <inheritdoc/>
    protected override int ExpectedLength => 27;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => MadagascarBbanValidator.Validate(bban);
}
