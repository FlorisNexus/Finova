using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.CentralAfricanRepublic.Validators;

/// <summary>
/// Validator for Central African Republic IBANs.
/// Central African Republic IBAN format: CF + 2 check digits + 23 digits BBAN.
/// Length: 27 characters.
/// </summary>
public class CentralAfricanRepublicIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "CF";

    /// <inheritdoc/>
    protected override int ExpectedLength => 27;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => CentralAfricanRepublicBbanValidator.Validate(bban);
}
