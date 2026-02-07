using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.Senegal.Validators;

/// <summary>
/// Validator for Senegal IBANs.
/// Senegal IBAN format: SN + 2 check digits + 24 characters (1 letter + 23 digits) BBAN.
/// Length: 28 characters.
/// </summary>
public class SenegalIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "SN";

    /// <inheritdoc/>
    protected override int ExpectedLength => 28;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => SenegalBbanValidator.Validate(bban);
}
