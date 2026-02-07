using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.GuineaBissau.Validators;

/// <summary>
/// Validator for Guinea-Bissau IBANs.
/// Guinea-Bissau IBAN format: GW + 2 check digits + 24 characters (1 letter + 23 digits) BBAN.
/// Length: 28 characters.
/// </summary>
public class GuineaBissauIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "GW";

    /// <inheritdoc/>
    protected override int ExpectedLength => 28;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => GuineaBissauBbanValidator.Validate(bban);
}
