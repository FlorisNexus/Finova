using System.Collections.Concurrent;
using Finova.Core.Common;
using Finova.Core.Vat;
using Finova.Countries.MiddleEast.Bahrain.Validators;
using Finova.Countries.MiddleEast.Israel.Validators;
using Finova.Countries.MiddleEast.Oman.Validators;
using Finova.Countries.MiddleEast.SaudiArabia.Validators;
using Finova.Countries.MiddleEast.UAE.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Services;

/// <summary>
/// Unified validator for Middle East VAT numbers.
/// Delegates validation to specific country validators based on the country code prefix.
/// </summary>
public class MiddleEastVatValidator : IVatValidator
{
    private static readonly ConcurrentDictionary<string, IVatValidator> _staticValidators = new();

    private readonly IServiceProvider? _serviceProvider;
    private IEnumerable<IVatValidator>? _validators;

    public MiddleEastVatValidator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public MiddleEastVatValidator()
    {
        _serviceProvider = null;
    }

    private IEnumerable<IVatValidator> GetValidators()
    {
        if (_validators == null && _serviceProvider != null)
        {
            _validators = _serviceProvider.GetServices<IVatValidator>()
                                          .Where(v => v.GetType() != typeof(MiddleEastVatValidator));
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
                "AE" => new UaeVatValidator(),
                "BH" => new BahrainVatValidator(),
                "IL" => new IsraelVatValidator(),
                "OM" => new OmanVatValidator(),
                "SA" => new SaudiArabiaVatValidator(),
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
            "QA" => Finova.Countries.MiddleEast.Qatar.Validators.QatarTinValidator.ValidateTin(vat),
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
            "BH" => new VatDetails { VatNumber = vat!, CountryCode = "BH", IsValid = true, IdentifierKind = "VAT" },
            "OM" => new VatDetails { VatNumber = vat!, CountryCode = "OM", IsValid = true, IdentifierKind = "VAT" },
            "QA" => new VatDetails { VatNumber = vat!, CountryCode = "QA", IsValid = true, IdentifierKind = "TIN" },
            _ => null
        };
    }
}