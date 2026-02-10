using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.MiddleEast.Israel.Validators;

/// <summary>
/// Validator for Israeli IBANs.
/// Israel IBAN format: IL + 2 check digits + 3 digits (bank code) + 3 digits (branch code) + 13 digits (account)
/// </summary>
public class IsraelIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "IL";

    /// <inheritdoc/>
    protected override int ExpectedLength => 23;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return IsraelBbanValidator.Validate(bban);
    }
}
