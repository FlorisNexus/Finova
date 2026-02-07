using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Sweden.Validators;

public class SwedenIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "SE";

    /// <inheritdoc/>
    protected override int ExpectedLength => 24;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => SwedenBbanValidator.Validate(bban);
}
