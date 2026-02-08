using Finova.Core.Common;
using Finova.Countries.NorthAmerica.Mexico.Validators;
using Finova.Countries.SouthAmerica.Argentina.Validators;
using Finova.Countries.SouthAmerica.Brazil.Validators;
using Finova.Countries.SouthAmerica.Chile.Validators;
using Finova.Countries.SouthAmerica.Colombia.Validators;

namespace Finova.Services.SouthAmerica;

public static class SouthAmericaTaxIdValidator
{
    public static ValidationResult Validate(string? taxId, string? countryCode = null)
    {
        if (string.IsNullOrWhiteSpace(taxId))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (string.IsNullOrWhiteSpace(countryCode))
        {
            if (taxId.Length < 2)
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.VatTooShortForCountryCode);
            }
            countryCode = taxId[0..2];
        }

        return countryCode.ToUpperInvariant() switch
        {
            "AR" => new ArgentinaCuitValidator().Validate(taxId),
            "BR" => BrazilCnpjValidator.ValidateCnpj(taxId),
            "CL" => new ChileRutValidator().Validate(taxId),
            "CO" => new ColombiaVatValidator().Validate(taxId),
            "MX" => MexicoRfcValidator.ValidateStatic(taxId),

            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountry)
        };
    }
}
