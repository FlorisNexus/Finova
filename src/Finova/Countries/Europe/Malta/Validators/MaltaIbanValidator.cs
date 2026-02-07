using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Malta.Validators;

/// <summary>
/// Validator for Maltese IBANs.
/// </summary>
public class MaltaIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "MT";

    /// <inheritdoc/>
    protected override int ExpectedLength => 31;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return MaltaBbanValidator.Validate(bban);
    }
}
