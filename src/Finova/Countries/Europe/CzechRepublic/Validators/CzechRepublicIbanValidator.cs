using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.CzechRepublic.Validators;

public class CzechRepublicIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "CZ";

    /// <inheritdoc/>
    protected override int ExpectedLength => 24;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => CzechRepublicBbanValidator.Validate(bban);
}
