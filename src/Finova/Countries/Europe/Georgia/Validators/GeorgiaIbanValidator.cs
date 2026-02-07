using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Georgia.Validators;

/// <summary>
/// Validator for Georgia IBANs.
/// </summary>
public class GeorgiaIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "GE";

    /// <inheritdoc/>
    protected override int ExpectedLength => 22;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => GeorgiaBbanValidator.Validate(bban);
}
