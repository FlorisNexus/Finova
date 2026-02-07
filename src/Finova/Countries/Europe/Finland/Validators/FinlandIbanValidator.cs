using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Finland.Validators;

public class FinlandIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "FI";

    /// <inheritdoc/>
    protected override int ExpectedLength => 18;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => FinlandBbanValidator.Validate(bban);
}
