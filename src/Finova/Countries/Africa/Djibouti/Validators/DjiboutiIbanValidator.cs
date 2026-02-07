using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.Djibouti.Validators;

/// <summary>
/// Validator for Djibouti IBANs.
/// Djibouti IBAN format: DJ + 2 check digits + 23 digits BBAN.
/// Length: 27 characters.
/// </summary>
public class DjiboutiIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "DJ";

    /// <inheritdoc/>
    protected override int ExpectedLength => 27;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => DjiboutiBbanValidator.Validate(bban);
}
