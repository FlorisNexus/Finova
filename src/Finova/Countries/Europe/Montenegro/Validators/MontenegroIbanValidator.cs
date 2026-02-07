using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Montenegro.Validators;

/// <summary>
/// Validator for Montenegro IBANs.
/// </summary>
public class MontenegroIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "ME";

    /// <inheritdoc/>
    protected override int ExpectedLength => 22;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => MontenegroBbanValidator.Validate(bban);
}
