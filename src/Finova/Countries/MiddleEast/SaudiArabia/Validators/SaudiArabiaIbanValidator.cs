using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.MiddleEast.SaudiArabia.Validators;

/// <summary>
/// Validator for Saudi Arabian IBANs.
/// Saudi Arabia IBAN format: SA + 2 check digits + 2 digits (bank code) + 18 alphanumeric (account)
/// </summary>
public class SaudiArabiaIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "SA";

    /// <inheritdoc/>
    protected override int ExpectedLength => 24;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return new SaudiArabiaBbanValidator().Validate(bban);
    }
}
