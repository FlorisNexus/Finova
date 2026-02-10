using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Asia.Mongolia.Validators;

/// <summary>
/// Validator for Mongolian IBANs.
/// Mongolia IBAN format: MN + 2 check digits + 16 digits BBAN.
/// </summary>
public class MongoliaIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "MN";

    /// <inheritdoc/>
    protected override int ExpectedLength => 20;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return MongoliaBbanValidator.Validate(bban);
    }
}
