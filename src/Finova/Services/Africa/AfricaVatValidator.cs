using System.Collections.Concurrent;
using Finova.Core.Common;
using Finova.Core.Vat;
using Finova.Countries.Africa.Algeria.Validators;
using Finova.Countries.Africa.Angola.Validators;
using Finova.Countries.Africa.CoteDIvoire.Validators;
using Finova.Countries.Africa.Egypt.Validators;
using Finova.Countries.Africa.Kenya.Validators;
using Finova.Countries.Africa.Morocco.Validators;
using Finova.Countries.Africa.Nigeria.Validators;
using Finova.Countries.Africa.Senegal.Validators;
using Finova.Countries.Africa.SouthAfrica.Validators;
using Finova.Countries.Africa.Tunisia.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Services;

/// <summary>
/// Unified validator for African VAT numbers.
/// Delegates validation to specific country validators based on the country code prefix.
/// </summary>
public class AfricaVatValidator : IVatValidator
{
    private static readonly ConcurrentDictionary<string, IVatValidator> _staticValidators = new();

    private readonly IServiceProvider? _serviceProvider;
    private IEnumerable<IVatValidator>? _validators;

    public AfricaVatValidator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public AfricaVatValidator()
    {
        _serviceProvider = null;
    }

    private IEnumerable<IVatValidator> GetValidators()
    {
        if (_validators == null && _serviceProvider != null)
        {
            _validators = _serviceProvider.GetServices<IVatValidator>()
                                          .Where(v => v.GetType() != typeof(AfricaVatValidator));
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

        var validator = _staticValidators.GetOrAdd(countryCode, code => code switch
        {
            "EG" => new EgyptVatValidator(),
            "KE" => new KenyaVatValidator(),
            "ZA" => new SouthAfricaVatValidator(),
            _ => null!
        });

        if (validator != null)
        {
            return validator.Validate(vat);
        }

        // Handle cases that don't have a standard class yet
        return countryCode switch
        {
            "AO" => new AngolaNifValidator().Validate(vat),
            "CI" => new IvoryCoastNccValidator().Validate(vat),
            "DZ" => new AlgeriaNifValidator().Validate(vat),
            "MA" => new MoroccoIceValidator().Validate(vat),
            "NG" => new NigeriaTinValidator().Validate(vat),
            "SN" => new SenegalNineaValidator().Validate(vat),
            "TN" => new TunisiaMatriculeFiscalValidator().Validate(vat),
            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountry)
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
            "AO" => new VatDetails { VatNumber = vat!, CountryCode = "AO", IsValid = true, IdentifierKind = "NIF" },
            "CI" => new VatDetails { VatNumber = vat!, CountryCode = "CI", IsValid = true, IdentifierKind = "NCC" },
            "DZ" => new VatDetails { VatNumber = vat!, CountryCode = "DZ", IsValid = true, IdentifierKind = "NIF" },
            "EG" => new VatDetails { VatNumber = vat!, CountryCode = "EG", IsValid = true, IdentifierKind = "TRN" },
            "MA" => new VatDetails { VatNumber = vat!, CountryCode = "MA", IsValid = true, IdentifierKind = "ICE" },
            "NG" => new VatDetails { VatNumber = vat!, CountryCode = "NG", IsValid = true, IdentifierKind = "TIN" },
            "SN" => new VatDetails { VatNumber = vat!, CountryCode = "SN", IsValid = true, IdentifierKind = "NINEA" },
            "TN" => new VatDetails { VatNumber = vat!, CountryCode = "TN", IsValid = true, IdentifierKind = "Matricule" },
            _ => null
        };
    }
}
