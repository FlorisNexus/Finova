using Finova.Services;
using Xunit;

namespace Finova.Tests.Validators;

public class VatExamplesTest
{
    [Theory]
    [InlineData("AT", "ATU33864707")]
    [InlineData("BE", "BE0202239951")]
    [InlineData("BG", "BG131468980")]
    [InlineData("HR", "HR81793146560")]
    [InlineData("CY", "CY30010823A")]
    [InlineData("CZ", "CZ00177041")]
    [InlineData("DK", "DK47458714")]
    [InlineData("EE", "EE100366327")]
    [InlineData("FI", "FI20584306")]
    [InlineData("FR", "FR33855200507")]
    [InlineData("DE", "DE129273398")]
    [InlineData("EL", "EL094019245")]
    [InlineData("HU", "HU17781774")]
    [InlineData("IE", "IE4749148U")]
    [InlineData("IT", "IT00159560366")]
    [InlineData("LV", "LV40003245752")]
    [InlineData("LT", "LT230335113")]
    [InlineData("LU", "LU18804375")]
    [InlineData("MT", "MT26758324")]
    [InlineData("NL", "NL001786519B01")]
    [InlineData("PL", "PL7342867148")]
    [InlineData("PT", "PT500278725")]
    [InlineData("RO", "RO160796")]
    [InlineData("SK", "SK2020317068")]
    [InlineData("SI", "SI82646716")]
    [InlineData("ES", "ESA28017895")]
    [InlineData("SE", "SE556056625801")]
    [InlineData("AL", "ALK31415037M")]
    [InlineData("AD", "ADU123456B")]
    [InlineData("AZ", "AZ1234567890")]
    [InlineData("BA", "BA4000000000005")]
    [InlineData("BY", "BY100000007")]
    [InlineData("FO", "FO123456")]
    [InlineData("GB", "GB220430231")]
    [InlineData("GE", "GE123456789")]
    [InlineData("IS", "IS12345")]
    [InlineData("XK", "XK123456782")]
    [InlineData("LI", "LI123456788")]
    [InlineData("MC", "FR44732829320")]
    [InlineData("MD", "MD1234567890123")]
    [InlineData("ME", "ME10000004")]
    [InlineData("MK", "MK4030992255006")]
    [InlineData("NO", "NO923609016MVA")]
    [InlineData("RS", "RS100000024")]
    [InlineData("CHE", "CHE105815381")]
    [InlineData("SM", "SM12345")]
    [InlineData("TR", "TR1234567890")]
    [InlineData("UA", "UA12345678")]
    public void VerifyVatExample(string countryCode, string vatNumber)
    {
        var result = EuropeVatValidator.ValidateVat(vatNumber, countryCode);
        Assert.True(result.IsValid, $"VAT {vatNumber} for {countryCode} is invalid: {string.Join(", ", result.Errors.Select(e => e.Message))}");
    }

    [Theory]
    [InlineData("VG", "VPVG0000012345678901")]
    [InlineData(null, "VPVG0000012345678901")]
    [InlineData("AU", "51824753556")]
    [InlineData(null, "AU51824753556")]
    [InlineData("RU", "7707083893")]
    [InlineData(null, "RU7707083893")]
    [InlineData("QA", "1234567890")]
    [InlineData(null, "QA1234567890")]
    // Middle East
    [InlineData("AE", "100111111111118")]      // Valid TRN with checksum (100 prefix, sum of digits % 10 gives check)
    [InlineData(null, "AE100111111111118")]
    [InlineData("SA", "300000000000007")]      // Valid Saudi VAT with Luhn checksum
    [InlineData(null, "SA300000000000007")]
    [InlineData("BH", "300000012345678")]
    [InlineData(null, "BH300000012345678")]
    [InlineData("IL", "516179157")]            // Valid Israeli VAT from test file
    [InlineData(null, "IL516179157")]
    [InlineData("OM", "123456789012345")]
    [InlineData(null, "OM123456789012345")]
    // Africa
    [InlineData("ZA", "4000000002")]           // Valid SA VAT (starts with 4, Luhn checksum)
    [InlineData(null, "ZA4000000002")]
    [InlineData("EG", "123456789")]
    [InlineData(null, "EG123456789")]
    [InlineData("KE", "P051123456Z")]
    [InlineData(null, "KEP051123456Z")]
    [InlineData("NG", "123456780001")]
    [InlineData(null, "NG123456780001")]
    [InlineData("MA", "001234567890123")]
    [InlineData(null, "MA001234567890123")]
    [InlineData("DZ", "000012345678901")]
    [InlineData(null, "DZ000012345678901")]
    [InlineData("TN", "1234567APM000")]
    [InlineData(null, "TN1234567APM000")]
    [InlineData("CI", "1234567A")]             // Valid Ivory Coast NCC (7 digits + 1 letter)
    [InlineData(null, "CI1234567A")]
    [InlineData("SN", "123456789012345")]      // Valid Senegal NINEA (15 digits)
    [InlineData(null, "SN123456789012345")]
    [InlineData("AO", "123456789")]            // Valid Angola NIF (9 digits)
    [InlineData(null, "AO123456789")]
    // Americas
    [InlineData("CA", "046454286RT0001")]      // Valid Canadian GST (046454286 = valid Luhn)
    [InlineData(null, "CA046454286RT0001")]
    [InlineData("AR", "20123456786")]          // Valid Argentina CUIT with correct checksum
    [InlineData(null, "AR20123456786")]
    [InlineData("BR", "11222333000181")]       // Valid Brazil CNPJ from test file
    [InlineData(null, "BR11222333000181")]
    [InlineData("CL", "123456785")]            // Valid Chile RUT (8 digits + check digit 5)
    [InlineData(null, "CL123456785")]
    [InlineData("CO", "123456789")]            // Valid Colombia NIT (9 digits)
    [InlineData(null, "CO123456789")]
    [InlineData("MX", "XAXX010101000")]        // Valid Mexico RFC from test file
    [InlineData(null, "MXXAXX010101000")]
    [InlineData("CR", "3101000000")]
    [InlineData(null, "CR3101000000")]
    [InlineData("DO", "101000001")]
    [InlineData(null, "DO101000001")]
    [InlineData("SV", "12345678901234")]
    [InlineData(null, "SV12345678901234")]
    [InlineData("GT", "1234567K")]
    [InlineData(null, "GT1234567K")]
    [InlineData("HN", "08011001123456")]
    [InlineData(null, "HN08011001123456")]
    [InlineData("NI", "J0310000000001")]
    [InlineData(null, "NIJ0310000000001")]
    // Asia
    [InlineData("CN", "91310000633518315Q")]
    [InlineData(null, "CN91310000633518315Q")]
    [InlineData("JP", "T2010401089234")]
    [InlineData(null, "JPT2010401089234")]
    [InlineData("KR", "1234567891")]           // Valid South Korea BRN with correct checksum
    [InlineData(null, "KR1234567891")]
    [InlineData("SG", "200312345A")]           // Valid Singapore UEN from test file
    [InlineData(null, "SG200312345A")]
    [InlineData("VN", "0100109106")]
    [InlineData(null, "VN0100109106")]
    [InlineData("KZ", "600900563762")]
    [InlineData(null, "KZ600900563762")]
    [InlineData("ID", "000000000000000")]      // Valid Indonesia NPWP (15 zeros, passes Luhn)
    [InlineData(null, "ID000000000000000")]
    [InlineData("PH", "123456789000")]
    [InlineData(null, "PH123456789000")]
    // Oceania
    [InlineData("NZ", "49091850")]
    [InlineData(null, "NZ49091850")]
    public void VerifyGlobalVatExample(string? countryCode, string vatNumber)
    {
        var result = GlobalVatValidator.ValidateVat(vatNumber, countryCode);
        Assert.True(result.IsValid, $"Global VAT {vatNumber} for {countryCode ?? "auto"} is invalid: {string.Join(", ", result.Errors.Select(e => e.Message))}");
    }
}
