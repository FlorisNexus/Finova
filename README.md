# Finova

<div align="center">

**The Offline Financial Validation Toolkit for .NET**

*IBAN · BIC · Payment Cards · Payment References · VAT · Business Numbers · Securities*

[![NuGet Version](https://img.shields.io/nuget/v/Finova.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/Finova/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Finova.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/Finova/)
[![Build Status](https://github.com/fdivrusa/Finova/actions/workflows/ci.yml/badge.svg)](https://github.com/fdivrusa/Finova/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-8.0%20%7C%2010.0-512BD4)](https://dotnet.microsoft.com/)

*100% Offline · Zero Dependencies · Lightning Fast*

[**🌐 Visit the Official Website**](https://www.finovasharp.com/)

</div>

---

## ✨ Why Finova?

| Feature | Benefit |
|---------|---------|
| 🆓 **Free Forever** | MIT License, no API keys, no usage limits, no subscription fees |
| ⚙️ **Enterprise Ready** | Production-grade validation trusted by financial institutions |
| 🔒 **Privacy First** | All validations run locally — your data never leaves your servers |
| ⚡ **Lightning Fast** | Optimized algorithms with zero network latency |
| 🌍 **Global Coverage** | 110+ countries across 6 continents |
| 🧩 **Easy Integration** | FluentValidation support, dependency injection ready |

---

## 📦 Quick Installation

```bash
# Main package
dotnet add package Finova

# FluentValidation integration (optional)
dotnet add package Finova.Extensions.FluentValidation
```

---

## 🚀 Feature Overview

### 🏦 Banking & Cards

| Validator | Description | Format |
|-----------|-------------|--------|
| **IBAN** | International Bank Account Number | `BE68 5390 0754 7034` |
| **BBAN** | Basic Bank Account Number | `539007547034` |
| **BIC/SWIFT** | Bank Identifier Code | `KREDBEBB` |
| **Payment Card** | Credit/Debit card (Visa, MC, Amex, etc.) | `4111 1111 1111 1111` |
| **Bank Routing** | US ABA, CA Transit Numbers | `021000021` |

### 📝 Payment References

| Format | Country | Example |
|--------|---------|---------|
| **Belgian OGM/VCS** | 🇧🇪 Belgium | `+++123/4567/89012+++` |
| **ISO RF** | 🌍 International | `RF18 5390 0754 7034` |
| **Finnish Viitenumero** | 🇫🇮 Finland | `12345 67890 00123` |
| **Norwegian KID** | 🇳🇴 Norway | `2345678901234` |
| **Swedish OCR** | 🇸🇪 Sweden | `1234567890136` |
| **Swiss QR-Reference** | 🇨🇭 Switzerland | `210000000003139471430009017` |
| **Slovenian SI12** | 🇸🇮 Slovenia | `SI12 1234 5678 901` |
| **Danish FIK** | 🇩🇰 Denmark | `+71<12345678901234+` |
| **Italian CBILL** | 🇮🇹 Italy | `12345 12345 12345 12345` |
| **Portuguese Multibanco** | 🇵🇹 Portugal | `12345 123456789 12` |

### 🏢 Business Numbers

| Validator | Description | Countries |
|-----------|-------------|-----------|
| **VAT Number** | Value Added Tax ID | 69 countries: All 27 EU + UK, CH, NO, Middle East (AE, BH, IL, OM, SA), Asia (CN, JP, KR, KZ, VN, PK, ID), Americas (AR, BR, CA, CL, CO, MX, US), Africa (ZA, KE), Oceania (NZ) and more |
| **Enterprise Number** | Business registration | 🇧🇪 Belgium (BCE/KBO), 🇱🇺 Luxembourg (RCS), 🇳🇱 Netherlands (KvK), 🇮🇪 Ireland (CRO), 🇲🇹 Malta (Company No.), 🇸🇰 Slovakia (IČO), 🇸🇮 Slovenia (Matična), 🇸🇪 Sweden (Org.nr), 🇿🇦 South Africa (CIPC), 🇳🇿 New Zealand (NZBN) |
| **EIN** | Employer Identification Number | 🇺🇸 USA |
| **LEI** | Legal Entity Identifier | 🌍 ISO 17442 worldwide |

### 🆔 National Identity

| Region | Countries | ID Types |
|--------|-----------|----------|
| **Western Europe** | BE, FR, DE, NL, UK, AT, CH | Belgian NN, French NIR, German Steuer-ID, Dutch BSN, UK NINO |
| **Northern Europe** | SE, NO, DK, FI, IS | Personnummer, Fødselsnummer, CPR, HETU, Kennitala |
| **Southern Europe** | IT, ES, PT, GR, TR | Codice Fiscale, DNI/NIE, NIF, AMKA, TC Kimlik |
| **Eastern Europe** | PL, RO, UA, BG, CZ, HU | PESEL, CNP, RNTRC, EGN, Rodné číslo |
| **Microstates** | AD, LI, MC, SM, VA | National ID formats |

### 📊 Securities Identifiers *(New in v1.4.0)*

| Validator | Description | Format | Example |
|-----------|-------------|--------|---------|
| **ISIN** | International Securities Identification Number | 2 letters + 9 chars + check digit | `US0378331005` (Apple) |
| **CUSIP** | Committee on Uniform Securities ID | 9 alphanumeric characters | `037833100` |
| **SEDOL** | Stock Exchange Daily Official List | 7 alphanumeric characters | `B0YQ5W0` |
| **Currency** | ISO 4217 currency code validation | 3 letters | `EUR`, `USD`, `GBP` |
| **LEI** | Legal Entity Identifier | 20 alphanumeric | `5493001KJTIIGC8Y1R12` |

---

## 🌍 Country Support

### Europe (51 Countries & Territories)

| Region | Countries |
|--------|-----------|
| **Western** | 🇧🇪 Belgium, 🇫🇷 France, 🇩🇪 Germany, 🇳🇱 Netherlands, 🇬🇧 UK, 🇦🇹 Austria, 🇨🇭 Switzerland, 🇱🇺 Luxembourg, 🇮🇪 Ireland, 🇲🇨 Monaco, 🇱🇮 Liechtenstein |
| **Northern** | 🇸🇪 Sweden, 🇳🇴 Norway, 🇩🇰 Denmark, 🇫🇮 Finland, 🇮🇸 Iceland, 🇪🇪 Estonia, 🇱🇻 Latvia, 🇱🇹 Lithuania, 🇫🇴 Faroe Islands, 🇬🇱 Greenland |
| **Southern** | 🇮🇹 Italy, 🇪🇸 Spain, 🇵🇹 Portugal, 🇬🇷 Greece, 🇲🇹 Malta, 🇨🇾 Cyprus, 🇦🇩 Andorra, 🇸🇲 San Marino, 🇻🇦 Vatican, 🇬🇮 Gibraltar |
| **Eastern** | 🇵🇱 Poland, 🇨🇿 Czech Republic, 🇸🇰 Slovakia, 🇭🇺 Hungary, 🇷🇴 Romania, 🇧🇬 Bulgaria, 🇸🇮 Slovenia, 🇭🇷 Croatia |
| **Balkans** | 🇷🇸 Serbia, 🇲🇪 Montenegro, 🇧🇦 Bosnia, 🇲🇰 North Macedonia, 🇦🇱 Albania, 🇽🇰 Kosovo |
| **East** | 🇺🇦 Ukraine, 🇲🇩 Moldova, 🇧🇾 Belarus, 🇬🇪 Georgia, 🇦🇿 Azerbaijan, 🇹🇷 Turkey |

### Global Expansion *(v1.4.0)*

| Region | Country | Tax ID | Bank Account | Bank Routing |
|--------|---------|--------|--------------|--------------|
| **North America** | 🇺🇸 USA | EIN ✓ | — | ABA Routing ✓ |
| | 🇨🇦 Canada | BN/GST ✓ | — | Transit Number ✓ |
| **Caribbean/Central America** | 🇨🇷 Costa Rica | NITE ✓ | IBAN ✓ | — |
| | 🇩🇴 Dominican Republic | RNC ✓ | IBAN ✓ | — |
| | 🇸🇻 El Salvador | NIT ✓ | IBAN ✓ | — |
| | 🇬🇹 Guatemala | NIT ✓ | IBAN ✓ | — |
| | 🇭🇳 Honduras | RTN ✓ | IBAN ✓ | — |
| | 🇳🇮 Nicaragua | RUC ✓ | IBAN ✓ | — |
| | 🇱🇨 Saint Lucia | Tax ID ✓ | IBAN ✓ | — |
| | 🇻🇬 Virgin Islands (British) | — | IBAN ✓ | — |
| | 🇧🇧 Barbados | Tax ID ✓ | IBAN ✓ | — |
| **South America** | 🇧🇷 Brazil | CNPJ/CPF ✓ | IBAN ✓ | — |
| | 🇲🇽 Mexico | RFC ✓ | — | — |
| | 🇦🇷 Argentina | CUIT/CUIL ✓ | — | — |
| | 🇨🇱 Chile | RUT ✓ | — | — |
| | 🇨🇴 Colombia | NIT ✓ | — | — |
| | 🇫🇰 Falkland Islands | — | IBAN ✓ | — |
| **Middle East** | 🇧🇭 Bahrain | VAT ✓ | IBAN ✓ | — |
| | 🇮🇶 Iraq | TIN ✓ | IBAN ✓ | — |
| | 🇮🇱 Israel | VAT ✓ | IBAN ✓ | — |
| | 🇯🇴 Jordan | TIN ✓ | IBAN ✓ | — |
| | 🇰🇼 Kuwait | TIN ✓ | IBAN ✓ | — |
| | 🇱🇧 Lebanon | TIN ✓ | IBAN ✓ | — |
| | 🇴🇲 Oman | VAT ✓ | IBAN ✓ | — |
| | 🇶🇦 Qatar | TIN ✓ | IBAN ✓ | — |
| | 🇸🇦 Saudi Arabia | VAT ✓ | IBAN ✓ | — |
| | 🇦🇪 UAE | VAT ✓ | IBAN ✓ | — |
| | 🇾🇪 Yemen | TIN ✓ | IBAN ✓ | — |
| **Africa** | 🇩🇿 Algeria | NIF ✓ | IBAN ✓ | — |
| | 🇦🇴 Angola | NIF ✓ | IBAN ✓ | — |
| | 🇧🇯 Benin | IFU ✓ | IBAN ✓ | — |
| | 🇧🇫 Burkina Faso | IFU ✓ | IBAN ✓ | — |
| | 🇧🇮 Burundi | NIF ✓ | IBAN ✓ | — |
| | 🇨🇲 Cameroon | NIU ✓ | IBAN ✓ | — |
| | 🇨🇻 Cape Verde | NIF ✓ | IBAN ✓ | — |
| | 🇨🇫 Central African Republic | NIF ✓ | IBAN ✓ | — |
| | 🇹🇩 Chad | NIF ✓ | IBAN ✓ | — |
| | 🇰🇲 Comoros | NIF ✓ | IBAN ✓ | — |
| | 🇨🇬 Congo | NIU ✓ | IBAN ✓ | — |
| | 🇨🇮 Côte d'Ivoire | NCC ✓ | IBAN ✓ | — |
| | 🇩🇯 Djibouti | NIF ✓ | IBAN ✓ | — |
| | 🇪🇬 Egypt | TRN ✓ | IBAN ✓ | — |
| | 🇬🇶 Equatorial Guinea | NIF ✓ | IBAN ✓ | — |
| | 🇬🇦 Gabon | NIF ✓ | IBAN ✓ | — |
| | 🇬🇼 Guinea-Bissau | NIF ✓ | IBAN ✓ | — |
| | 🇱🇾 Libya | Tax ID ✓ | IBAN ✓ | — |
| | 🇲🇦 Morocco | ICE/IF ✓ | IBAN ✓ | — |
| | 🇲🇬 Madagascar | NIF ✓ | IBAN ✓ | — |
| | 🇲🇱 Mali | NIF ✓ | IBAN ✓ | — |
| | 🇲🇷 Mauritania | NIF ✓ | IBAN ✓ | — |
| | 🇲🇿 Mozambique | NUIT ✓ | IBAN ✓ | — |
| | 🇳🇪 Niger | NIF ✓ | IBAN ✓ | — |
| | 🇿🇦 South Africa | CIPC ✓ | — | — |
| | 🇸🇹 Sao Tome and Principe | NIF ✓ | IBAN ✓ | — |
| | 🇸🇳 Senegal | NINEA ✓ | IBAN ✓ | — |
| | 🇸🇨 Seychelles | TIN ✓ | IBAN ✓ | — |
| | 🇸🇴 Somalia | TIN ✓ | IBAN ✓ | — |
| | 🇸🇩 Sudan | TIN ✓ | IBAN ✓ | — |
| | 🇹🇬 Togo | NIF ✓ | IBAN ✓ | — |
| | 🇹🇳 Tunisia | MF ✓ | IBAN ✓ | — |
| | 🇰🇪 Kenya | PIN ✓ | — | — |
| **Asia** | 🇨🇳 China | USCC ✓ | — | — |
| | 🇯🇵 Japan | Corporate ✓ | — | — |
| | 🇮🇳 India | GSTIN/PAN ✓ | — | — |
| | 🇸🇬 Singapore | UEN ✓ | — | — |
| | 🇰🇿 Kazakhstan | BIN/IIN ✓ | IBAN ✓ | — |
| | 🇲🇳 Mongolia | TIN ✓ | IBAN ✓ | — |
| | 🇵🇰 Pakistan | NTN ✓ | — | — |
| | 🇹🇱 Timor-Leste | TIN ✓ | IBAN ✓ | — |
| **Europe (Non-SEPA)** | 🇷🇺 Russia | INN ✓ | IBAN ✓ | — |
| **Southeast Asia** | 🇮🇩 Indonesia | NPWP ✓ | — | — |
| | 🇲🇾 Malaysia | TIN ✓ | — | — |
| | 🇹🇭 Thailand | TIN ✓ | — | — |
| | 🇻🇳 Vietnam | MST ✓ | — | — |
| **Oceania** | 🇦🇺 Australia | ABN/TFN ✓ | — | BSB ✓ |
| | 🇳🇿 New Zealand | NZBN ✓ | — | — |

---

## 🔧 Usage Examples

### Basic Validation

```csharp
using Finova.Core.Iban;
using Finova.Core.PaymentCard;
using Finova.Core.Identifiers;
using Finova.Services;

// Global IBAN Validation (supports all IBAN-enabled countries worldwide)
var result = GlobalIbanValidator.ValidateIban("BR1800360305000010009795493C1");
if (result.IsValid)
{
    Console.WriteLine("Valid Brazilian IBAN!");
}

// Country-specific IBAN Validation
var ibanService = new IbanService();
var result = ibanService.Validate("BE68 5390 0754 7034");

if (result.IsValid)
{
    var details = ibanService.Parse("BE68 5390 0754 7034");
    Console.WriteLine($"Country: {details.CountryCode}");
    Console.WriteLine($"BBAN: {details.Bban}");
    Console.WriteLine($"Check Digits: {details.CheckDigits}");
}

// BBAN Validation (Basic Bank Account Number)
var bbanService = serviceProvider.GetRequiredService<IBbanService>();
var bbanResult = bbanService.Validate("BE", "539007547034");

if (bbanResult.IsValid)
{
    var bbanDetails = bbanService.Parse("BE", "539007547034");
    Console.WriteLine($"Country: {bbanDetails.CountryCode}"); // BE
    Console.WriteLine($"BBAN: {bbanDetails.Bban}");           // 539007547034
}

// Payment Card Validation
var cardValidator = new PaymentCardValidator();
var cardResult = cardValidator.Validate("4111111111111111");

if (cardResult.IsValid)
{
    var cardDetails = cardValidator.Parse("4111111111111111");
    Console.WriteLine($"Brand: {cardDetails.Brand}"); // Visa
}

// ISIN Validation (Securities)
var isinValidator = new IsinValidator();
var isinResult = isinValidator.Validate("US0378331005");

if (isinResult.IsValid)
{
    var isinDetails = isinValidator.Parse("US0378331005");
    Console.WriteLine($"Country: {isinDetails.CountryCode}"); // US
    Console.WriteLine($"NSIN: {isinDetails.Nsin}");           // 037833100
}
```

### Dependency Injection

```csharp
using Finova.Extensions;

// Register all Finova services
services.AddFinova();

// Or register specific services
services.AddFinovaCore();
services.AddFinovaCountry<BelgiumModule>();
```

```csharp
public class PaymentService
{
    private readonly IIbanService _ibanService;
    private readonly IVatValidator _vatValidator;

    public PaymentService(IIbanService ibanService, IVatValidator vatValidator)
    {
        _ibanService = ibanService;
        _vatValidator = vatValidator;
    }

    public bool ValidatePaymentDetails(string iban, string vatNumber)
    {
        return _ibanService.Validate(iban).IsValid
            && _vatValidator.Validate(vatNumber).IsValid;
    }
}
```

### FluentValidation Integration

```csharp
using Finova.Extensions.FluentValidation;

public class PaymentRequestValidator : AbstractValidator<PaymentRequest>
{
    public PaymentRequestValidator()
    {
        // 🏦 Banking
        RuleFor(x => x.Iban).MustBeValidIban();
        RuleFor(x => x.Bic).MustBeValidBic();

        // 🇺🇸🇨🇦🇦🇺🇸🇬🇯🇵 Routing Numbers (Aliases)
        RuleFor(x => x.UsRoutingNumber).MustBeValidABARoutingNumber();       // US ABA
        RuleFor(x => x.CaTransitNumber).MustBeValidCanadianTransitNumber(); // Canada
        RuleFor(x => x.AuBsb).MustBeValidAustralianBsb();                   // Australia BSB
        RuleFor(x => x.SgBankCode).MustBeValidSingaporeBankCode();          // Singapore
        RuleFor(x => x.JpZengin).MustBeValidJapanZenginCode();              // Japan

        // 💳 Payment Cards
        RuleFor(x => x.CardNumber).MustBeValidPaymentCard();
        RuleFor(x => x.VisaNumber).MustBeValidPaymentCardForBrand(PaymentCardBrand.Visa);

        // 🏢 Enterprise & Tax IDs (Aliases)
        RuleFor(x => x.Siret).MustBeValidSiret();                           // France SIRET
        RuleFor(x => x.Siren).MustBeValidSiren();                           // France SIREN
        RuleFor(x => x.Kbo).MustBeValidKbo();                               // Belgium KBO/BCE
        RuleFor(x => x.VatNumber).MustBeValidVat();                         // EU VAT

        // 📝 Payment References
        RuleFor(x => x.Communication).MustBeValidIsoRf();                   // ISO 11649 RF
        RuleFor(x => x.Ogm).MustBeValidOgm();                               // Belgian OGM
        RuleFor(x => x.QrRef).MustBeValidSwissQrReference();                // Swiss QR
    }
}
```

### Country-Specific Validation

```csharp
using Finova.Countries.Belgium;
using Finova.Countries.UnitedStates;

// Belgian validations
var belgiumIban = new BelgiumIbanValidator();
belgiumIban.Validate("BE68 5390 0754 7034"); // ✓

var belgiumVat = new BelgiumVatValidator();
belgiumVat.Validate("BE0123456789"); // ✓

var belgiumNationalId = new BelgiumNationalIdValidator();
belgiumNationalId.Validate("85.07.30-033.28"); // ✓

// US validations
var usRouting = new UnitedStatesRoutingNumberValidator();
usRouting.Validate("021000021"); // ✓

var usEin = new UnitedStatesEinValidator();
usEin.Validate("12-3456789"); // ✓
```

---

## 📋 Validation Rules Reference

### IBAN Structure

| Component | Length | Description |
|-----------|--------|-------------|
| Country Code | 2 | ISO 3166-1 alpha-2 |
| Check Digits | 2 | Mod 97 validation |
| BBAN | Variable | Country-specific format |

### Payment Card Validation

| Brand | Prefix | Length | Algorithm |
|-------|--------|--------|-----------|
| Visa | 4 | 16 | Luhn |
| Mastercard | 51-55, 2221-2720 | 16 | Luhn |
| American Express | 34, 37 | 15 | Luhn |
| Discover | 6011, 644-649, 65 | 16 | Luhn |
| Diners Club | 36, 38, 300-305 | 14-16 | Luhn |

### ISIN Structure

| Component | Position | Description |
|-----------|----------|-------------|
| Country Code | 1-2 | ISO 3166-1 alpha-2 |
| NSIN | 3-11 | National Security ID |
| Check Digit | 12 | Luhn mod 10 |

---

## 📊 Test Coverage

```
Total Tests: 10,700+
Passing: 100%
Countries Covered: 110+
Validators: 100+
```

---

## 🗺️ Roadmap

| Version | Status | Features |
|---------|--------|----------|
| v1.0.0 | ✅ Released | Core IBAN, BIC, Payment Cards |
| v1.1.0 | ✅ Released | Payment References (BE, FI, NO, SE, CH) |
| v1.2.0 | ✅ Released | VAT validation, Enterprise Numbers |
| v1.3.0 | ✅ Released | National ID (51 European countries) |
| **v1.4.0** | 🚀 **Current** | Global Expansion, Securities (ISIN, CUSIP, SEDOL), Currency |
| v1.5.0 | 📋 Planned | EPC QR Code generation, SEPA XML |
| v1.6.0 | 📋 Planned | Async validation, Batch processing |

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 🤝 Contributing

Contributions are welcome! Please read our [Contributing Guidelines](CONTRIBUTING.md) for details.

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

---

## 🙏 Acknowledgments

- Financial standards: ISO 13616 (IBAN), ISO 9362 (BIC), ISO 6166 (ISIN), ISO 17442 (LEI)
- European Banking Authority for IBAN registry
- GLEIF for LEI standards
- Community contributors

---

<div align="center">

**Made with ❤️ by [Florian Di Vrusa](https://github.com/fdivrusa)**

[⭐ Star on GitHub](https://github.com/fdivrusa/Finova) · [📦 NuGet Package](https://www.nuget.org/packages/Finova) · [🌐 Website](https://www.finovasharp.com)

</div>
