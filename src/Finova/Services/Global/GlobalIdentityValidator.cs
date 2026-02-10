using System.Collections.Concurrent;
using Finova.Core.Common;
using Finova.Core.Identifiers;
using Finova.Countries.Africa.Egypt.Validators;
using Finova.Countries.Africa.Kenya.Validators;
using Finova.Countries.Africa.Nigeria.Validators;
using Finova.Countries.Africa.SouthAfrica.Validators;
using Finova.Countries.Asia.China.Validators;
using Finova.Countries.Asia.India.Validators;
using Finova.Countries.Asia.Japan.Validators;
using Finova.Countries.Asia.Singapore.Validators;
using Finova.Countries.Asia.SouthKorea.Validators;
using Finova.Countries.Europe.Russia.Validators;
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
using Finova.Countries.MiddleEast.Israel.Validators;
using Finova.Countries.MiddleEast.SaudiArabia.Validators;
using Finova.Countries.MiddleEast.UAE.Validators;
using Finova.Countries.NorthAmerica.Canada.Validators;
using Finova.Countries.NorthAmerica.UnitedStates.Validators;
using Finova.Countries.Oceania.Australia.Validators;
using Finova.Countries.SouthAmerica.Brazil.Validators;
using Finova.Countries.SouthAmerica.Argentina.Validators;
using Finova.Countries.SouthAmerica.Chile.Validators;
using Finova.Countries.SouthAmerica.Colombia.Validators;
using Finova.Countries.NorthAmerica.Mexico.Validators;
using Finova.Countries.SoutheastAsia.Indonesia.Validators;
using Finova.Countries.SoutheastAsia.Malaysia.Validators;
using Finova.Countries.SoutheastAsia.Thailand.Validators;
using Finova.Countries.SoutheastAsia.Vietnam.Validators;

namespace Finova.Services;

/// <summary>
/// Unified validator for Global Identity Numbers (National ID, Tax ID).
/// Delegates validation to specific country validators.
/// </summary>
public static class GlobalIdentityValidator
{
    private static readonly ConcurrentDictionary<string, INationalIdValidator> _nationalIdValidators = new();
    private static readonly ConcurrentDictionary<string, ITaxIdValidator> _taxIdValidators = new();

    /// <summary>
    /// Validates a National ID for the specified country.
    /// </summary>
    /// <param name="countryCode">The 2-letter ISO country code.</param>
    /// <param name="nationalId">The National ID to validate.</param>
    public static ValidationResult ValidateNationalId(string countryCode, string? nationalId)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string country = countryCode.ToUpperInvariant();

        if (!_nationalIdValidators.TryGetValue(country, out var validator))
        {
            validator = country switch
            {
                "AL" => new AlbaniaNationalIdValidator(),
                "AD" => new AndorraNationalIdValidator(),
                "AT" => new AustriaNationalIdValidator(),
                "AZ" => new AzerbaijanNationalIdValidator(),
                "BY" => new BelarusNationalIdValidator(),
                "BE" => new BelgiumNationalIdValidator(),
                "BA" => new BosniaAndHerzegovinaNationalIdValidator(),
                "BG" => new BulgariaNationalIdValidator(),
                "HR" => new CroatiaOibValidator(),
                "CY" => new CyprusNationalIdValidator(),
                "CZ" => new CzechRepublicNationalIdValidator(),
                "DK" => new DenmarkCprValidator(),
                "EE" => new EstoniaNationalIdValidator(),
                "FO" => new FaroeIslandsNationalIdValidator(),
                "FI" => new FinlandHenkilotunnusValidator(),
                "FR" => new FranceNationalIdValidator(),
                "GE" => new GeorgiaNationalIdValidator(),
                "DE" => new GermanyNationalIdValidator(),
                "GI" => new GibraltarNationalIdValidator(),
                "GR" or "EL" => new GreeceNationalIdValidator(),
                "GL" => new GreenlandNationalIdValidator(),
                "HU" => new HungaryNationalIdValidator(),
                "IS" => new IcelandKennitalaValidator(),
                "IE" => new IrelandNationalIdValidator(),
                "IT" => new ItalyNationalIdValidator(),
                "XK" => new KosovoNationalIdValidator(),
                "LV" => new LatviaNationalIdValidator(),
                "LI" => new LiechtensteinNationalIdValidator(),
                "LT" => new LithuaniaNationalIdValidator(),
                "LU" => new LuxembourgNationalIdValidator(),
                "MT" => new MaltaNationalIdValidator(),
                "MD" => new MoldovaNationalIdValidator(),
                "MC" => new MonacoNationalIdValidator(),
                "ME" => new MontenegroNationalIdValidator(),
                "NL" => new NetherlandsNationalIdValidator(),
                "MK" => new NorthMacedoniaNationalIdValidator(),
                "NO" => new NorwayNationalIdValidator(),
                "PL" => new PolandNationalIdValidator(),
                "PT" => new PortugalNationalIdValidator(),
                "RO" => new RomaniaNationalIdValidator(),
                "SM" => new SanMarinoNationalIdValidator(),
                "RS" => new SerbiaNationalIdValidator(),
                "SK" => new SlovakiaNationalIdValidator(),
                "SI" => new SloveniaNationalIdValidator(),
                "ES" => new SpainNationalIdValidator(),
                "SE" => new SwedenNationalIdValidator(),
                "CH" => new SwitzerlandNationalIdValidator(),
                "TR" => new TurkeyNationalIdValidator(),
                "UA" => new UkraineNationalIdValidator(),
                "GB" or "UK" => new UnitedKingdomNationalIdValidator(),
                "VA" => new VaticanNationalIdValidator(),
                "RU" => new RussiaNationalIdValidator(),
                "CN" => new ChinaResidentIdentityCardValidator(),
                "JP" => new JapanMyNumberValidator(),
                "SG" => new SingaporeNricValidator(),
                "IL" => new IsraelTeudatZehutValidator(),
                "SA" => new SaudiArabiaIdValidator(),
                "AE" => new UaeEmiratesIdValidator(),
                "IN" => new IndiaAadhaarValidator(),
                "KR" => new SouthKoreaNationalIdValidator(),
                "US" => new UnitedStatesSsnValidator(),
                "CA" => new CanadaSinValidator(),
                "NG" => new NigeriaNinValidator(),
                "ZA" => new SouthAfricaIdValidator(),
                "BR" => new BrazilCpfValidator(),
                _ => null
            };

            if (validator != null)
            {
                _nationalIdValidators.TryAdd(country, validator);
            }
        }

        if (validator != null)
        {
            return validator.Validate(nationalId);
        }

        // Handle cases that don't have a standard class yet
        return country switch
        {
            "EG" => EgyptNationalIdValidator.ValidateStatic(nationalId),
            "KE" => KenyaNationalIdValidator.ValidateStatic(nationalId),
            "ID" => IndonesiaNikValidator.ValidateStatic(nationalId),
            "MY" => MalaysiaMyKadValidator.ValidateStatic(nationalId),
            "TH" => ThailandIdValidator.ValidateStatic(nationalId),
            "VN" => VietnamCitizenIdValidator.ValidateStatic(nationalId),
            "AR" => ArgentinaCuitValidator.ValidateStatic(nationalId),
            "CL" => ChileRutValidator.ValidateStatic(nationalId),
            "CO" => ColombiaCedulaValidator.ValidateStatic(nationalId),
            "MX" => MexicoCurpValidator.ValidateStatic(nationalId),
            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountry)
        };
    }

    /// <summary>
    /// Validates a Tax ID for the specified country.
    /// </summary>
    /// <param name="countryCode">The 2-letter ISO country code.</param>
    /// <param name="taxId">The Tax ID to validate.</param>
    public static ValidationResult ValidateTaxId(string countryCode, string? taxId)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string country = countryCode.ToUpperInvariant();

        if (!_taxIdValidators.TryGetValue(country, out var validator))
        {
            validator = country switch
            {
                "IN" => new IndiaPanValidator(),
                "CA" => new CanadaBusinessNumberValidator(),
                "US" => new UnitedStatesEinValidator(),
                "BR" => new BrazilCnpjValidator(),
                "CO" => new ColombiaVatValidator(),
                "KZ" => new Finova.Countries.Asia.Kazakhstan.Validators.KazakhstanBinValidator(),
                "VN" => new VietnamTaxIdValidator(),
                "EG" => new EgyptTaxRegistrationNumberValidator(),
                "MA" => new Finova.Countries.Africa.Morocco.Validators.MoroccoIceValidator(),
                "DZ" => new Finova.Countries.Africa.Algeria.Validators.AlgeriaNifValidator(),
                "TN" => new Finova.Countries.Africa.Tunisia.Validators.TunisiaMatriculeFiscalValidator(),
                "NG" => new Finova.Countries.Africa.Nigeria.Validators.NigeriaTinValidator(),
                _ => null
            };

            if (validator != null)
            {
                _taxIdValidators.TryAdd(country, validator);
            }
        }

        if (validator != null)
        {
            return validator.Validate(taxId);
        }

        return country switch
        {
            "CN" => ChinaUnifiedSocialCreditCodeValidator.ValidateUscc(taxId),
            "AU" => ValidateAustraliaTaxId(taxId),
            "AR" => ArgentinaCuitValidator.ValidateStatic(taxId),
            "CL" => ChileRutValidator.ValidateStatic(taxId),
            "MX" => MexicoRfcValidator.ValidateStatic(taxId),
            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountry)
        };
    }

    private static ValidationResult ValidateAustraliaTaxId(string? taxId)
    {
        var tfnResult = new AustraliaTfnValidator().Validate(taxId);
        if (tfnResult.IsValid)
        {
            return tfnResult;
        }

        var abnResult = new AustraliaAbnValidator().Validate(taxId);
        if (abnResult.IsValid)
        {
            return abnResult;
        }

        return tfnResult;
    }
}
