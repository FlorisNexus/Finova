using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.FaroeIslands.Validators;

/// <summary>
/// Validator for Faroe Islands IBANs.
/// </summary>
public class FaroeIslandsIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "FO";

    /// <inheritdoc/>
    protected override int ExpectedLength => 18;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => FaroeIslandsBbanValidator.Validate(bban);
}
