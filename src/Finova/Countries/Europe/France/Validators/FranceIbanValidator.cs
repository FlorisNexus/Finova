using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.France.Validators;

/// <summary>
/// Validator for French IBANs.
/// France IBAN format: FR + 2 check digits + 5 bank code + 5 branch code + 11 account number + 2 RIB key.
/// </summary>
public class FranceIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "FR";

    /// <inheritdoc/>
    protected override int ExpectedLength => 27;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return FranceBbanValidator.Validate(bban);
    }
}
