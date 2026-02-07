using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Monaco.Validators;

public class MonacoIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "MC";

    /// <inheritdoc/>
    protected override int ExpectedLength => 27;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => MonacoBbanValidator.Validate(bban);
}
