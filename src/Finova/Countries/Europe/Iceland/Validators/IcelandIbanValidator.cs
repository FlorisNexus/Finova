using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Iceland.Validators;

public class IcelandIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "IS";

    /// <inheritdoc/>
    protected override int ExpectedLength => 26;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => IcelandBbanValidator.Validate(bban);
}
