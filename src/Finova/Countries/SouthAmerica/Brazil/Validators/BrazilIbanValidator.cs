using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.SouthAmerica.Brazil.Validators;

/// <summary>
/// Validator for Brazilian IBANs.
/// Brazil IBAN format: BR + 2 check digits + 8 digits (bank code) + 5 digits (branch) + 10 digits (account) + 1 letter (account type) + 1 letter (owner)
/// </summary>
public class BrazilIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "BR";

    /// <inheritdoc/>
    protected override int ExpectedLength => 29;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return BrazilBbanValidator.Validate(bban);
    }
}
