using System.Collections.Concurrent;
using Finova.Core.Common;
using Finova.Core.Iban;
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
using Finova.Countries.Europe.Gibraltar.Validators;
using Finova.Countries.Europe.Greece.Validators;
using Finova.Countries.Europe.Greenland.Validators;
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
using Finova.Countries.Europe.Vatican.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Services;

/// <summary>
/// Unified validator for European IBANs.
/// Delegates validation to specific country validators based on the country code prefix.
/// </summary>
/// <example>
/// <code>
/// // Static usage
/// var result = EuropeIbanValidator.ValidateIban("BE68539007547034");
///
/// // Instance usage (DI)
/// var validator = new EuropeIbanValidator();
/// var result = validator.Validate("FR7630006000011234567890189");
/// </code>
/// </example>
public class EuropeIbanValidator : IIbanValidator
{
    private static readonly ConcurrentDictionary<string, IIbanValidator> _staticValidators = new();

    private readonly IServiceProvider? _serviceProvider;
    private IEnumerable<IIbanValidator>? _validators;

    public EuropeIbanValidator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public EuropeIbanValidator()
    {
        _serviceProvider = null;
    }

    private IEnumerable<IIbanValidator> GetValidators()
    {
        if (_validators == null && _serviceProvider != null)
        {
            _validators = _serviceProvider.GetServices<IIbanValidator>()
                                          .Where(v => v.GetType() != typeof(EuropeIbanValidator));
        }
        return _validators ?? [];
    }

    public string CountryCode => "";

    public ValidationResult Validate(string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban) || iban.Length < 2)
        {
            return ValidateIban(iban); // Fallback to static validation for basic checks
        }

        string countryCode = iban[0..2].ToUpperInvariant();
        var validators = GetValidators().Where(v => v.CountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase)).ToList();

        if (validators.Count > 0)
        {
            var result = validators.Select(validator => validator.Validate(iban))
                                   .FirstOrDefault(r => r.IsValid);

            if (result != null)
            {
                return result;
            }
        }

        // Fallback to static logic if no specific validator is found in DI
        return ValidateIban(iban);
    }

    public static ValidationResult ValidateIban(string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (iban.Length < 2)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InvalidLength);
        }

        string country = IbanHelper.NormalizeIban(iban)[0..2].ToUpperInvariant();

        var validator = _staticValidators.GetOrAdd(country, code => code switch
        {
            "DE" => new GermanyIbanValidator(),
            "IT" => new ItalyIbanValidator(),
            "ES" => new SpainIbanValidator(),
            "FR" => new FranceIbanValidator(),
            "BE" => new BelgiumIbanValidator(),
            "NL" => new NetherlandsIbanValidator(),
            "GB" => new UnitedKingdomIbanValidator(),
            "LU" => new LuxembourgIbanValidator(),
            "IE" => new IrelandIbanValidator(),
            "AT" => new AustriaIbanValidator(),
            "GR" => new GreeceIbanValidator(),
            "FI" => new FinlandIbanValidator(),
            "PT" => new PortugalIbanValidator(),
            "SE" => new SwedenIbanValidator(),
            "DK" => new DenmarkIbanValidator(),
            "NO" => new NorwayIbanValidator(),
            "PL" => new PolandIbanValidator(),
            "CZ" => new CzechRepublicIbanValidator(),
            "HU" => new HungaryIbanValidator(),
            "RO" => new RomaniaIbanValidator(),
            "BG" => new BulgariaIbanValidator(),
            "HR" => new CroatiaIbanValidator(),
            "SI" => new SloveniaIbanValidator(),
            "SK" => new SlovakiaIbanValidator(),
            "EE" => new EstoniaIbanValidator(),
            "LV" => new LatviaIbanValidator(),
            "LT" => new LithuaniaIbanValidator(),
            "CY" => new CyprusIbanValidator(),
            "MT" => new MaltaIbanValidator(),
            "CH" => new SwitzerlandIbanValidator(),
            "MC" => new MonacoIbanValidator(),
            "AD" => new AndorraIbanValidator(),
            "VA" => new VaticanIbanValidator(),
            "SM" => new SanMarinoIbanValidator(),
            "GI" => new GibraltarIbanValidator(),
            "IS" => new IcelandIbanValidator(),
            "LI" => new LiechtensteinIbanValidator(),
            "RS" => new SerbiaIbanValidator(),
            "UA" => new UkraineIbanValidator(),
            "ME" => new MontenegroIbanValidator(),
            "MK" => new NorthMacedoniaIbanValidator(),
            "AL" => new AlbaniaIbanValidator(),
            "TR" => new TurkeyIbanValidator(),
            "BA" => new BosniaAndHerzegovinaIbanValidator(),
            "GE" => new GeorgiaIbanValidator(),
            "FO" => new FaroeIslandsIbanValidator(),
            "GL" => new GreenlandIbanValidator(),
            "XK" => new KosovoIbanValidator(),
            "MD" => new MoldovaIbanValidator(),
            "BY" => new BelarusIbanValidator(),
            "AZ" => new AzerbaijanIbanValidator(),
            _ => null!
        });

        return validator != null
            ? validator.Validate(iban)
            : ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountryOrInvalidIban);
    }
}
