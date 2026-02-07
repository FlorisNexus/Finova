using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.Tunisia.Validators;

/// <summary>
/// Validator for Tunisia IBANs.
/// Tunisia IBAN format: TN + 2 check digits + 20 digits BBAN.
/// Length: 24 characters.
/// </summary>
public class TunisiaIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "TN";

    /// <inheritdoc/>
    protected override int ExpectedLength => 24;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => TunisiaBbanValidator.Validate(bban);
}
