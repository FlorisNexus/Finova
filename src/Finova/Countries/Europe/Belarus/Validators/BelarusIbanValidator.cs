using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Belarus.Validators;

/// <summary>
/// Validator for Belarus IBANs.
/// </summary>
public class BelarusIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "BY";

    /// <inheritdoc/>
    protected override int ExpectedLength => 28;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => BelarusBbanValidator.Validate(bban);
}
