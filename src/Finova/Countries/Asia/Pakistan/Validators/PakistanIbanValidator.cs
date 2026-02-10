using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Asia.Pakistan.Validators;

/// <summary>
/// Validator for Pakistani IBANs.
/// Pakistan IBAN format: PK + 2 check digits + 4 letters (bank code) + 16 digits (account)
/// </summary>
public class PakistanIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "PK";

    /// <inheritdoc/>
    protected override int ExpectedLength => 24;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return PakistanBbanValidator.Validate(bban);
    }
}
