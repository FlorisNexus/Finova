using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.MiddleEast.Oman.Validators;

/// <summary>
/// Validator for Omani IBANs.
/// Oman IBAN format: OM + 2 check digits + 19 digits BBAN.
/// </summary>
public class OmanIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "OM";

    /// <inheritdoc/>
    protected override int ExpectedLength => 23;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return OmanBbanValidator.Validate(bban);
    }
}
