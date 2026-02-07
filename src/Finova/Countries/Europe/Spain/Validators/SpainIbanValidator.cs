using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Spain.Validators;

/// <summary>
/// Validator for Spanish IBANs.
/// Format: ES (2) + Check (2) + Entidad (4) + Oficina (4) + DC (2) + Cuenta (10).
/// </summary>
public class SpainIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "ES";

    /// <inheritdoc/>
    protected override int ExpectedLength => 24;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return SpainBbanValidator.Validate(bban);
    }
}
