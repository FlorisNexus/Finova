using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.NorthAmerica.DominicanRepublic.Validators;

/// <summary>
/// Validator for Dominican Republic IBANs.
/// Dominican Republic IBAN format: DO + 2 check digits + 4 letters (bank code) + 20 digits (account)
/// </summary>
public class DominicanRepublicIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "DO";

    /// <inheritdoc/>
    protected override int ExpectedLength => 28;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return DominicanRepublicBbanValidator.Validate(bban);
    }
}
