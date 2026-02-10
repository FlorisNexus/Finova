using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.NorthAmerica.CostaRica.Validators;

/// <summary>
/// Validator for Costa Rican IBANs.
/// Costa Rica IBAN format: CR + 2 check digits + 1 digit (reserve) + 3 digits (bank code) + 14 digits (account)
/// </summary>
public class CostaRicaIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "CR";

    /// <inheritdoc/>
    protected override int ExpectedLength => 22;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return CostaRicaBbanValidator.Validate(bban);
    }
}
