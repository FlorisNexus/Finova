using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.NorthAmerica.Guatemala.Validators;

/// <summary>
/// Validator for Guatemalan IBANs.
/// Guatemala IBAN format: GT + 2 check digits + 4 letters (bank code) + 20 alphanumeric (account)
/// </summary>
public class GuatemalaIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "GT";

    /// <inheritdoc/>
    protected override int ExpectedLength => 28;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return GuatemalaBbanValidator.Validate(bban);
    }
}
