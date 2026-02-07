using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Cyprus.Validators;

public class CyprusIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "CY";

    /// <inheritdoc/>
    protected override int ExpectedLength => 28;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => CyprusBbanValidator.Validate(bban);
}
