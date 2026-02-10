using System.Collections.Concurrent;
using Finova.Core.Common;
using Finova.Core.Vat;
using Finova.Countries.NorthAmerica.Canada.Validators;
using Finova.Countries.NorthAmerica.CostaRica.Validators;
using Finova.Countries.NorthAmerica.DominicanRepublic.Validators;
using Finova.Countries.NorthAmerica.ElSalvador.Validators;
using Finova.Countries.NorthAmerica.Guatemala.Validators;
using Finova.Countries.NorthAmerica.Honduras.Validators;
using Finova.Countries.NorthAmerica.Nicaragua.Validators;
using Finova.Countries.SouthAmerica.Argentina.Validators;
using Finova.Countries.SouthAmerica.Brazil.Validators;
using Finova.Countries.SouthAmerica.Chile.Validators;
using Finova.Countries.SouthAmerica.Colombia.Validators;
using Finova.Countries.NorthAmerica.Mexico.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Services;

/// <summary>
/// Unified validator for Americas VAT/GST numbers.
/// Delegates validation to specific country validators based on the country code prefix.
/// </summary>
public class AmericasVatValidator : IVatValidator
{
    private static readonly ConcurrentDictionary<string, IVatValidator> _staticValidators = new();

    private readonly IServiceProvider? _serviceProvider;
    private IEnumerable<IVatValidator>? _validators;

    public AmericasVatValidator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public AmericasVatValidator()
    {
        _serviceProvider = null;
    }

    private IEnumerable<IVatValidator> GetValidators()
    {
        if (_validators == null && _serviceProvider != null)
        {
            _validators = _serviceProvider.GetServices<IVatValidator>()
                                          .Where(v => v.GetType() != typeof(AmericasVatValidator));
        }
        return _validators ?? Enumerable.Empty<IVatValidator>();
    }

    public string CountryCode => "";

    public ValidationResult Validate(string? input)
    {
        if (string.IsNullOrWhiteSpace(input) || input.Length < 2)
        {
            return ValidateVat(input);
        }

        string countryCode = input[0..2].ToUpperInvariant();
        var validator = GetValidators().FirstOrDefault(v => v.CountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase));

        if (validator != null)
        {
            return validator.Validate(input);
        }

        return ValidateVat(input);
    }

    public VatDetails? Parse(string? input)
    {
        if (string.IsNullOrWhiteSpace(input) || input.Length < 2)
        {
            return GetVatDetails(input);
        }

        string countryCode = input[0..2].ToUpperInvariant();
        var validator = GetValidators().FirstOrDefault(v => v.CountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase));

        if (validator != null)
        {
            return validator.Parse(input);
        }

        return GetVatDetails(input);
    }

    public static ValidationResult ValidateVat(string? vat, string? countryCode = null)
    {
        vat = VatSanitizer.Sanitize(vat);

        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (string.IsNullOrWhiteSpace(countryCode))
        {
            if (vat.Length < 2)
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.VatTooShortForCountryCode);
            }
            countryCode = vat[0..2];
        }

        countryCode = countryCode.ToUpperInvariant();

        if (!_staticValidators.TryGetValue(countryCode, out var validator))
        {
            validator = countryCode switch
            {
                "AR" => new ArgentinaVatValidator(),
                "BR" => new BrazilVatValidator(),
                "CL" => new ChileVatValidator(),
                "CO" => new ColombiaVatValidator(),
                "MX" => new MexicoVatValidator(),
                _ => null
            };

            if (validator != null)
            {
                _staticValidators.TryAdd(countryCode, validator);
            }
        }

        if (validator != null)
        {
            return validator.Validate(vat);
        }

        return countryCode switch
        {
            "CA" => CanadaGstValidator.ValidateStatic(vat),
            "CR" => CostaRicaNiteValidator.ValidateNite(vat),
            "DO" => DominicanRepublicRncValidator.ValidateRnc(vat),
            "SV" => ElSalvadorNitValidator.ValidateNit(vat),
            "GT" => GuatemalaNitValidator.ValidateNit(vat),
            "HN" => HondurasRtnValidator.ValidateRtn(vat),
            "NI" => NicaraguaRucValidator.ValidateRuc(vat),
            "VG" or "VP" => ValidationResult.Success(),
            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, string.Format(ValidationMessages.UnsupportedCountryCodeFormat, countryCode))
        };
    }

    public static VatDetails? GetVatDetails(string? vat, string? countryCode = null)
    {
        vat = VatSanitizer.Sanitize(vat);

        if (string.IsNullOrWhiteSpace(vat))
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(countryCode))
        {
            if (vat.Length < 2)
            {
                return null;
            }
            countryCode = vat[0..2];
        }

        countryCode = countryCode.ToUpperInvariant();

        var validator = _staticValidators.GetValueOrDefault(countryCode);
        if (validator != null)
        {
            return validator.Parse(vat);
        }

        return countryCode switch
        {
            "CA" => CanadaGstValidator.GetVatDetails(vat),
            "CR" => new VatDetails { VatNumber = vat!, CountryCode = "CR", IsValid = true, IdentifierKind = "NITE" },
            "DO" => new VatDetails { VatNumber = vat!, CountryCode = "DO", IsValid = true, IdentifierKind = "RNC" },
            "SV" => new VatDetails { VatNumber = vat!, CountryCode = "SV", IsValid = true, IdentifierKind = "NIT" },
            "GT" => new VatDetails { VatNumber = vat!, CountryCode = "GT", IsValid = true, IdentifierKind = "NIT" },
            "HN" => new VatDetails { VatNumber = vat!, CountryCode = "HN", IsValid = true, IdentifierKind = "RTN" },
            "NI" => new VatDetails { VatNumber = vat!, CountryCode = "NI", IsValid = true, IdentifierKind = "RUC" },
            "VG" or "VP" => new VatDetails { VatNumber = vat!, CountryCode = "VG", IsValid = true, IdentifierKind = "None" },
            _ => null
        };
    }
}