using System.Collections.Concurrent;
using Finova.Core.Common;
using Finova.Core.Vat;
using Finova.Countries.Asia.China.Validators;
using Finova.Countries.Asia.India.Validators;
using Finova.Countries.Asia.Japan.Validators;
using Finova.Countries.Asia.Kazakhstan.Validators;
using Finova.Countries.Asia.Singapore.Validators;
using Finova.Countries.Asia.SouthKorea.Validators;
using Finova.Countries.SoutheastAsia.Indonesia.Validators;
using Finova.Countries.SoutheastAsia.Philippines.Validators;
using Finova.Countries.SoutheastAsia.Vietnam.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Services;

/// <summary>
/// Unified validator for Asian VAT/GST numbers.
/// Delegates validation to specific country validators based on the country code prefix.
/// </summary>
public class AsiaVatValidator : IVatValidator
{
    private static readonly ConcurrentDictionary<string, IVatValidator> _staticValidators = new();

    private readonly IServiceProvider? _serviceProvider;
    private IEnumerable<IVatValidator>? _validators;

    public AsiaVatValidator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public AsiaVatValidator()
    {
        _serviceProvider = null;
    }

    private IEnumerable<IVatValidator> GetValidators()
    {
        if (_validators == null && _serviceProvider != null)
        {
            _validators = _serviceProvider.GetServices<IVatValidator>()
                                          .Where(v => v.GetType() != typeof(AsiaVatValidator));
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
            "CN" => new ChinaVatValidator(),
            "ID" => new IndonesiaVatValidator(),
            "IN" => new IndiaGstinValidator(),
            "JP" => new JapanVatValidator(),
            "KR" => new SouthKoreaVatValidator(),
            "PH" => new PhilippinesVatValidator(),
            "SG" => new SingaporeGstValidator(),
            "VN" => new VietnamVatValidator(),
            _ => null!
        });

        if (validator != null)
        {
            return validator.Validate(vat);
        }

        return countryCode switch
        {
            "KZ" => new KazakhstanBinValidator().Validate(vat),
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
            "VN" => new VatDetails { VatNumber = vat!, CountryCode = "VN", IsValid = true, IdentifierKind = "MST" },
            "KZ" => new VatDetails { VatNumber = vat!, CountryCode = "KZ", IsValid = true, IdentifierKind = "BIN" },
            _ => null
        };
    }
}