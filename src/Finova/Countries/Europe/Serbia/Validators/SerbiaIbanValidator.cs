using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Serbia.Validators;

/// <summary>
/// Validator for Serbian IBANs.
/// </summary>
public class SerbiaIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "RS";

    /// <inheritdoc/>
    protected override int ExpectedLength => 22;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return SerbiaBbanValidator.Validate(bban);
    }
}
