using System.Collections.Concurrent;
using Finova.Core.Common;
using Finova.Core.Vat;
using Finova.Countries.Europe.Albania.Validators;
using Finova.Countries.Europe.Andorra.Validators;
using Finova.Countries.Europe.Austria.Validators;
using Finova.Countries.Europe.Azerbaijan.Validators;
using Finova.Countries.Europe.Belarus.Validators;
using Finova.Countries.Europe.Belgium.Validators;
using Finova.Countries.Europe.BosniaAndHerzegovina.Validators;
using Finova.Countries.Europe.Bulgaria.Validators;
using Finova.Countries.Europe.Croatia.Validators;
using Finova.Countries.Europe.Cyprus.Validators;
using Finova.Countries.Europe.CzechRepublic.Validators;
using Finova.Countries.Europe.Denmark.Validators;
using Finova.Countries.Europe.Estonia.Validators;
using Finova.Countries.Europe.FaroeIslands.Validators;
using Finova.Countries.Europe.Finland.Validators;
using Finova.Countries.Europe.France.Validators;
using Finova.Countries.Europe.Georgia.Validators;
using Finova.Countries.Europe.Germany.Validators;
using Finova.Countries.Europe.Greece.Validators;
using Finova.Countries.Europe.Hungary.Validators;
using Finova.Countries.Europe.Iceland.Validators;
using Finova.Countries.Europe.Ireland.Validators;
using Finova.Countries.Europe.Italy.Validators;
using Finova.Countries.Europe.Kosovo.Validators;
using Finova.Countries.Europe.Latvia.Validators;
using Finova.Countries.Europe.Liechtenstein.Validators;
using Finova.Countries.Europe.Lithuania.Validators;
using Finova.Countries.Europe.Luxembourg.Validators;
using Finova.Countries.Europe.Malta.Validators;
using Finova.Countries.Europe.Moldova.Validators;
using Finova.Countries.Europe.Monaco.Validators;
using Finova.Countries.Europe.Montenegro.Validators;
using Finova.Countries.Europe.Netherlands.Validators;
using Finova.Countries.Europe.NorthMacedonia.Validators;
using Finova.Countries.Europe.Norway.Validators;
using Finova.Countries.Europe.Poland.Validators;
using Finova.Countries.Europe.Portugal.Validators;
using Finova.Countries.Europe.Romania.Validators;
using Finova.Countries.Europe.SanMarino.Validators;
using Finova.Countries.Europe.Serbia.Validators;
using Finova.Countries.Europe.Slovakia.Validators;
using Finova.Countries.Europe.Slovenia.Validators;
using Finova.Countries.Europe.Spain.Validators;
using Finova.Countries.Europe.Sweden.Validators;
using Finova.Countries.Europe.Switzerland.Validators;
using Finova.Countries.Europe.Turkey.Validators;
using Finova.Countries.Europe.Ukraine.Validators;
using Finova.Countries.Europe.UnitedKingdom.Validators;

using Microsoft.Extensions.DependencyInjection;

namespace Finova.Services;

/// <summary>
/// Unified validator for European VAT numbers.
/// Delegates validation to specific country validators based on the country code prefix.
/// </summary>
public class EuropeVatValidator : IVatValidator
{
    private static readonly ConcurrentDictionary<string, IVatValidator> _staticValidators = new();

    private readonly IServiceProvider? _serviceProvider;
    private IEnumerable<IVatValidator>? _validators;

    public EuropeVatValidator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public EuropeVatValidator()
    {
        _serviceProvider = null;
    }

    private IEnumerable<IVatValidator> GetValidators()
    {
        if (_validators == null && _serviceProvider != null)
        {
            _validators = _serviceProvider.GetServices<IVatValidator>()
                                          .Where(v => v.GetType() != typeof(EuropeVatValidator));
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
        var validators = GetValidators().Where(v => v.CountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase)).ToList();

        if (validators.Count > 0)
        {
            var result = validators.Select(validator => validator.Parse(input))
                                   .FirstOrDefault(r => r != null);

            if (result != null)
            {
                return result;
            }
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
            "AL" => new AlbaniaVatValidator(),
            "AD" => new AndorraVatValidator(),
            "AT" => new AustriaVatValidator(),
            "AZ" => new AzerbaijanVatValidator(),
            "BA" => new BosniaAndHerzegovinaVatValidator(),
            "BE" => new BelgiumVatValidator(),
            "BG" => new BulgariaVatValidator(),
            "BY" => new BelarusVatValidator(),
            "CHE" or "CH" => new SwitzerlandVatValidator(),
            "CY" => new CyprusVatValidator(),
            "CZ" => new CzechRepublicVatValidator(),
            "DE" => new GermanyVatValidator(),
            "DK" => new DenmarkVatValidator(),
            "EE" => new EstoniaVatValidator(),
            "EL" or "GR" => new GreeceVatValidator(),
            "ES" => new SpainVatValidator(),
            "FI" => new FinlandVatValidator(),
            "FO" => new FaroeIslandsVatValidator(),
            "FR" => new FranceVatValidator(),
            "GB" => new UnitedKingdomVatValidator(),
            "GE" => new GeorgiaVatValidator(),
            "HR" => new CroatiaVatValidator(),
            "HU" => new HungaryVatValidator(),
            "IE" => new IrelandVatValidator(),
            "IS" => new IcelandVatValidator(),
            "IT" => new ItalyVatValidator(),
            "XK" => new KosovoVatValidator(),
            "LI" => new LiechtensteinVatValidator(),
            "LT" => new LithuaniaVatValidator(),
            "LU" => new LuxembourgVatValidator(),
            "LV" => new LatviaVatValidator(),
            "MC" => new MonacoVatValidator(),
            "MD" => new MoldovaVatValidator(),
            "ME" => new MontenegroVatValidator(),
            "MK" => new NorthMacedoniaVatValidator(),
            "MT" => new MaltaVatValidator(),
            "NL" => new NetherlandsVatValidator(),
            "NO" => new NorwayVatValidator(),
            "PL" => new PolandVatValidator(),
            "PT" => new PortugalVatValidator(),
            "RO" => new RomaniaVatValidator(),
            "RS" => new SerbiaVatValidator(),
            "RU" => new Finova.Countries.Europe.Russia.Validators.RussiaVatValidator(),
            "SE" => new SwedenVatValidator(),
            "SI" => new SloveniaVatValidator(),
            "SK" => new SlovakiaVatValidator(),
            "SM" => new SanMarinoVatValidator(),
            "TR" => new Finova.Countries.Europe.Turkey.Validators.TurkeyVatValidator(),
            "UA" => new UkraineVatValidator(),
            _ => null!
        });

        if (validator != null)
        {
            return validator.Validate(vat);
        }

        return ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, $"Country code {countryCode} is not supported.");
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

        ValidationResult result = ValidateVat(vat, countryCode);
        if (!result.IsValid)
        {
            return null;
        }

        // For most validators, Parse will work
        var validator = _staticValidators.GetValueOrDefault(countryCode);
        VatDetails? details = null;

        if (validator != null)
        {
            details = validator.Parse(vat);
        }

        if (details != null)
        {
            // Normalize country code for metadata lookup (handle aliases)
            string lookupCode = countryCode;
            if (lookupCode == "CHE")
            {
                lookupCode = "CH";
            }
            if (lookupCode == "GR")
            {
                lookupCode = "EL";
            }

            var metadata = EuropeCountryIdentifierMetadata.For(lookupCode);
            if (metadata != null)
            {
                return details with
                {
                    IdentifierKind = metadata.VatKind.ToString(),
                    IsEuVat = metadata.IsEuVat,
                    IsViesEligible = metadata.IsViesEligible,
                    Notes = metadata.Note
                };
            }
        }

        return details;
    }
}