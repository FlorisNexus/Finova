using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Austria.Validators;

public class AustriaIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "AT";

    /// <inheritdoc/>
    protected override int ExpectedLength => 20;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => AustriaBbanValidator.Validate(bban);
}
