using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.NorthAmerica.Barbados.Validators;

/// <summary>
/// Validator for Barbadian IBANs.
/// Barbados IBAN format: BB + 2 check digits + 24 characters BBAN.
/// </summary>
public class BarbadosIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "BB";

    /// <inheritdoc/>
    protected override int ExpectedLength => 28;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return BarbadosBbanValidator.Validate(bban);
    }
}
