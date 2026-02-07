using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.CapeVerde.Validators;

/// <summary>
/// Validator for Cape Verde IBANs.
/// Cape Verde IBAN format: CV + 2 check digits + 21 digits BBAN.
/// Length: 25 characters.
/// </summary>
public class CapeVerdeIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "CV";

    /// <inheritdoc/>
    protected override int ExpectedLength => 25;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => CapeVerdeBbanValidator.Validate(bban);
}
