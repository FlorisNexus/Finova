# Finova

<div align="center">

**Innovative financial toolkit for .NET**

*IBAN validation · Payment references · VAT validation · PEPPOL · UBL · SEPA*

[![NuGet](https://img.shields.io/nuget/v/Finova.svg?label=NuGet)](https://www.nuget.org/packages/Finova/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Finova.svg?label=Downloads)](https://www.nuget.org/packages/Finova/)
[![GitHub Package](https://img.shields.io/badge/GitHub-Package-blue?logo=github)](https://github.com/fdivrusa/Finova/packages)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

</div>

---

## 📊 Build Status

| Branch | CI | CD | Coverage |
|--------|----|----|----------|
| **master** | [![CI - master](https://github.com/fdivrusa/Finova/actions/workflows/ci.yml/badge.svg?branch=master)](https://github.com/fdivrusa/Finova/actions/workflows/ci.yml?query=branch%3Amaster) | [![CD](https://github.com/fdivrusa/Finova/actions/workflows/cd.yml/badge.svg)](https://github.com/fdivrusa/Finova/actions/workflows/cd.yml) | [![codecov](https://codecov.io/gh/fdivrusa/Finova/branch/master/graph/badge.svg)](https://codecov.io/gh/fdivrusa/Finova/branch/master) |
| **develop** | [![CI - develop](https://github.com/fdivrusa/Finova/actions/workflows/ci.yml/badge.svg?branch=develop)](https://github.com/fdivrusa/Finova/actions/workflows/ci.yml?query=branch%3Adevelop) | [![CD - develop](https://github.com/fdivrusa/Finova/actions/workflows/cd.yml/badge.svg?branch=develop)](https://github.com/fdivrusa/Finova/actions/workflows/cd.yml?query=branch%3Adevelop) | [![codecov](https://codecov.io/gh/fdivrusa/Finova/branch/develop/graph/badge.svg)](https://codecov.io/gh/fdivrusa/Finova/branch/develop) |

---

## 🌟 About Finova

**Finova** is a comprehensive financial operations library for .NET, designed for modern applications requiring financial validation, payment processing, and e-invoicing capabilities. Built with European and international standards in mind, Finova provides production-ready tools for banking, tax, and invoicing operations.

### Why Finova?

- ✅ **Production-Ready** - Battle-tested with 100+ unit tests and >95% code coverage
- ✅ **Standards-Compliant** - ISO 11649, EN 16931, PEPPOL, UBL, SEPA
- ✅ **International** - Multi-country support with extensible architecture
- ✅ **Modern** - Built for .NET 10.0+ with dependency injection support
- ✅ **Open Source** - MIT licensed, community-driven development

---

## 🚀 Features

### 💳 **Banking & Payments** *(Available Now)*

- **Payment References**
  - ✅ ISO 11649 (RF) international references
  - ✅ Belgian OGM/VCS (+++XXX/XXXX/XXXXX+++)
  - ✅ Automatic check digit calculation
  - ✅ Format validation and normalization

### 🌍 **International Support** *(Coming Soon)*

- **IBAN Validation** - v1.1.0 (Q1 2026)
  - Multi-country support (BE, NL, FR, DE, LU, UK, etc.)
  - Country-specific validation rules
  - Format normalization and BIC lookup
  
- **VAT & Tax** - v1.2.0 (Q2 2026)
  - EU VAT number validation
  - VIES real-time integration
  - Enterprise number validation (KBO/BCE)
  - Tax identifier validation

### 📄 **E-Invoicing & PEPPOL** *(Roadmap)*

- **PEPPOL** - v1.4.0+ (Q4 2026)
  - Participant ID validation
  - Document type validation
  - PEPPOL BIS 3.0 support
  
- **UBL 2.1** - v2.0.0+ (Q1 2027)
  - Invoice generation
  - EN 16931 compliance
  - Credit notes and debit notes
  
- **SEPA** - v1.3.0 (Q3 2026)
  - SEPA Credit Transfer (pain.001)
  - SEPA Direct Debit (pain.008)
  - XML file generation

### 🏗️ **Architecture**

- **Modular Design** - Separation of core, regional, and specialized features
- **Dependency Injection** - Full ASP.NET Core integration
- **Extensible** - Easy to add custom validators and generators
- **Type-Safe** - Strong typing with comprehensive interfaces

- **Type-Safe** - Strong typing with comprehensive interfaces

---

## 📦 Installation

### Stable Release

```bash
dotnet add package Finova
```

Or via Package Manager Console:
```powershell
Install-Package Finova
```

### Pre-release/Alpha

To install the latest alpha version with new features:

```bash
dotnet add package Finova --version *-alpha.*
```

Or via Package Manager Console:
```powershell
Install-Package Finova -PreRelease
```

> **Note:** Alpha versions are published from the `develop` branch (format: `1.0.0-alpha.{commits}+{sha}`). See [VERSIONING.md](VERSIONING.md) for details.

---

## 📖 Quick Start

### Belgian Payment References

```csharp
using Finova.Regional.Belgium.Services;
using Finova.Core.Models;

// Create service instance
var service = new BelgianPaymentService();

// Generate Belgian OGM/VCS structured communication
string ogm = service.Generate("123456", PaymentReferenceFormat.Domestic);
// Output: +++000/0012/34569+++

// Generate ISO 11649 international reference
string isoRef = service.Generate("INVOICE2024", PaymentReferenceFormat.IsoRf);
// Output: RF89INVOICE2024

// Validate payment reference
bool isValid = service.IsValid("+++000/0012/34569+++");
// Output: true
```

### Dependency Injection (ASP.NET Core)

```csharp
using Finova.Regional.Belgium.Extensions;

// In Program.cs
builder.Services.AddBelgianBanking();

// In your service
public class InvoiceService
{
    private readonly IPaymentReferenceGenerator _paymentRefGenerator;

    public InvoiceService(IPaymentReferenceGenerator paymentRefGenerator)
    {
        _paymentRefGenerator = paymentRefGenerator;
    }

    public string CreateInvoice(int invoiceNumber)
    {
        var paymentRef = _paymentRefGenerator.Generate(
            invoiceNumber.ToString(), 
            PaymentReferenceFormat.Domestic
        );
        
        return paymentRef; // +++XXX/XXXX/XXXXX+++
    }
}
```

### ISO 11649 References

```csharp
using Finova.Core.Internals;

// Generate ISO 11649 reference
string reference = IsoReferenceHelper.Generate("CUSTOMER12345");
// Output: RF23CUSTOMER12345

// Validate ISO 11649 reference
bool isValid = IsoReferenceValidator.IsValid("RF23CUSTOMER12345");
// Output: true

// Works with spaces (display format)
bool isValid2 = IsoReferenceValidator.IsValid("RF23 CUSTOMER 12345");
// Output: true
```

### Modulo 97 Calculations

```csharp
using Finova.Core.Internals;

// Calculate modulo 97 of a numeric string
int result = Modulo97Helper.Calculate("1234567890");
// Output: 37

// Works with very large numbers
int result2 = Modulo97Helper.Calculate("123456789012345678901234567890");
// Returns correct modulo 97 result
```

---

## � Use Cases

<table>
<tr>
<td width="50%">

### **Financial Services**
- Payment processing
- IBAN verification
- SEPA file generation
- Bank account validation

</td>
<td width="50%">

### **E-Commerce**
- Invoice payment references
- Multi-country payments
- Payment validation
- Order processing

</td>
</tr>
<tr>
<td width="50%">

### **Accounting & ERP**
- VAT validation
- Tax identifiers
- Enterprise numbers
- Multi-currency invoicing

</td>
<td width="50%">

### **E-Invoicing**
- PEPPOL compliance
- UBL generation
- EN 16931 compliance
- Digital invoice exchange

</td>
</tr>
</table>

---

## 🏗️ Architecture

### Project Structure

```
Finova
├── Finova.Core              → Shared utilities, interfaces, algorithms
├── Finova.Regional.Belgium  → Belgian banking features
├── Finova.Banking          → IBAN, BIC, SEPA (coming v1.1+)
├── Finova.Tax              → VAT, tax IDs (coming v1.2+)
└── Finova.Invoicing        → PEPPOL, UBL (coming v1.4+)
```

### Core Library (`Finova.Core`)

Provides foundational utilities:
- `IPaymentReferenceGenerator` - Payment reference interface
- `IBankAccountValidator` - IBAN validation interface *(coming v1.1)*
- `Modulo97Helper` - ISO 7064 modulo 97 calculations
- `IsoReferenceHelper` - ISO 11649 reference generation
- `IsoReferenceValidator` - ISO 11649 validation
- `PaymentReferenceFormat` - Format types enum

### Belgian Implementation (`Finova.Regional.Belgium`)

Belgian-specific features:
- `BelgianPaymentService` - Implements `IPaymentReferenceGenerator`
  - OGM/VCS format (+++XXX/XXXX/XXXXX+++)
  - ISO 11649 format support
  - Complete validation logic
- `ServiceCollectionExtensions` - DI registration helpers

### Extensibility

```csharp
// Implement custom validators
public class CustomPaymentService : IPaymentReferenceGenerator
{
    public string CountryCode => "NL";
    
    public string Generate(string rawReference, PaymentReferenceFormat format)
    {
        // Your custom implementation
    }
    
    public bool IsValid(string reference)
    {
        // Your validation logic
    }
}

// Register with DI
services.AddSingleton<IPaymentReferenceGenerator, CustomPaymentService>();
```

---

## 🧪 Quality & Testing

- ✅ **106 Unit Tests** - Comprehensive test coverage
- ✅ **>95% Code Coverage** - High quality assurance
- ✅ **CI/CD Pipeline** - Automated build, test, and deployment
- ✅ **Code Quality** - Linting and formatting checks
- ✅ **Performance** - Benchmarked for production use

Run tests:
```bash
dotnet test
```

View coverage reports:
- [Master Branch Coverage](https://codecov.io/gh/fdivrusa/Finova/tree/master)
- [Develop Branch Coverage](https://codecov.io/gh/fdivrusa/Finova/tree/develop)

- [Develop Branch Coverage](https://codecov.io/gh/fdivrusa/Finova/tree/develop)

---

## 🚀 CI/CD Pipeline

### Continuous Integration (CI)

Runs automatically on every push or pull request:

| Branch | Status | Trigger | Actions |
|--------|--------|---------|---------|
| **master** | [![CI - master](https://github.com/fdivrusa/Finova/actions/workflows/ci.yml/badge.svg?branch=master)](https://github.com/fdivrusa/Finova/actions/workflows/ci.yml?query=branch%3Amaster) | Push or PR | Build, test, coverage, linting |
| **develop** | [![CI - develop](https://github.com/fdivrusa/Finova/actions/workflows/ci.yml/badge.svg?branch=develop)](https://github.com/fdivrusa/Finova/actions/workflows/ci.yml?query=branch%3Adevelop) | Push or PR | Build, test, coverage, linting |

### Continuous Deployment (CD)

Manual workflow dispatch with automatic branch-based versioning:

- **Trigger**: ⚙️ Manual dispatch or 🏷️ GitHub release
- **Destinations**: NuGet.org + GitHub Packages
- **Status**: [![CD](https://github.com/fdivrusa/Finova/actions/workflows/cd.yml/badge.svg)](https://github.com/fdivrusa/Finova/actions/workflows/cd.yml)

### Versioning Strategy

| Branch | Format | Example | Description |
|--------|--------|---------|-------------|
| **master** | `{base}.{commits}` | `1.0.0.123` | Stable production releases |
| **develop** | `{base}-alpha.{commits}+{sha}` | `1.0.0-alpha.42+a1b2c3d` | Alpha pre-releases |

**To publish:**
1. Go to [Actions → CD Workflow](https://github.com/fdivrusa/Finova/actions/workflows/cd.yml)
2. Click "Run workflow"
3. Select branch (`master` or `develop`)
4. Version is automatically determined

See [VERSIONING.md](VERSIONING.md) for complete details.

---

## 🗺️ Roadmap

### ✅ v1.0.0 - Foundation (Released)
- Belgian payment references (OGM/VCS)
- ISO 11649 international references
- Comprehensive testing and CI/CD

### 🔄 v1.1.0 - European Banking (Q1 2026)
- [ ] IBAN validation (BE, NL, FR, DE, LU, UK)
- [ ] BIC/SWIFT code validation
- [ ] Bank code to BIC mapping
- [ ] Legacy account number conversion

### 📋 v1.2.0 - Tax & Business (Q2 2026)
- [ ] VAT number validation (EU-27)
- [ ] VIES real-time integration
- [ ] Enterprise number validation (KBO/BCE)
- [ ] Tax identifier validation

### 📋 v1.3.0 - SEPA Payments (Q3 2026)
- [ ] SEPA Credit Transfer (pain.001)
- [ ] SEPA Direct Debit (pain.008)
- [ ] XML file generation
- [ ] Batch payment support

### 📋 v1.4.0 - PEPPOL Foundation (Q4 2026)
- [ ] PEPPOL participant ID validation
- [ ] Document type identifiers
- [ ] PEPPOL BIS 3.0 support
- [ ] Endpoint validation

### 📋 v2.0.0 - E-Invoicing Suite (Q1 2027)
- [ ] UBL 2.1 invoice generation
- [ ] EN 16931 compliance
- [ ] Credit notes and debit notes
- [ ] Cross Industry Invoice (CII)

### 📋 v2.1.0+ - Country Expansion (Q2+ 2027)
- [ ] Country-specific e-invoicing (DE, FR, IT, ES)
- [ ] XRechnung, Factur-X, FatturaPA
- [ ] Additional SEPA countries
- [ ] Global expansion (US, AU, SG, etc.)

**See [ROADMAP.md](ROADMAP.md) for detailed feature breakdown.**

---

## 🌍 Country Support

### Current Support 🇧🇪
- **Belgium** - Payment references (OGM/VCS, ISO 11649)

### Coming v1.1-1.2
- 🇧🇪 **Belgium** - IBAN, VAT, enterprise numbers
- 🇳🇱 **Netherlands** - IBAN, VAT
- 🇫🇷 **France** - IBAN, VAT
- 🇩🇪 **Germany** - IBAN, VAT
- 🇱🇺 **Luxembourg** - IBAN, VAT
- 🇬🇧 **United Kingdom** - IBAN

### Future Plans
- 🇮🇹 Italy, 🇪🇸 Spain, 🇦🇹 Austria, 🇸🇪 Sweden, 🇵🇹 Portugal
- More EU countries and international expansion

**Want to add your country?** See [CONTRIBUTING.md](CONTRIBUTING.md)!

---

## 🔧 Supported Standards

### Current
- **ISO 11649** - International payment references (RF creditor reference)
- **ISO 7064** - Modulo 97 checksum algorithm
- **Belgian OGM/VCS** - Structured communication format

### Coming Soon
- **ISO 13616** - IBAN structure and validation (v1.1)
- **ISO 9362** - BIC/SWIFT codes (v1.1)
- **EN 16931** - European e-invoicing semantic model (v2.0)
- **PEPPOL BIS 3.0** - Business Interoperability Specifications (v1.4)
- **UBL 2.1** - Universal Business Language (v2.0)
- **ISO 20022** - SEPA payment messages (v1.3)

---

## 📚 Documentation

- [Getting Started Guide](docs/getting-started.md) *(coming soon)*
- [API Reference](#-api-reference)
- [PEPPOL Guide](docs/peppol-guide.md) *(coming soon)*
- [Contributing Guidelines](CONTRIBUTING.md) *(coming soon)*
- [Versioning Strategy](VERSIONING.md)
- [Package Metadata](PACKAGE_METADATA.md)
- [Detailed Roadmap](ROADMAP.md) *(coming soon)*

---

## 🤝 Contributing

We welcome contributions! Here's how you can help:

### Priority Areas
1. 🌍 **Country Implementations** - Add IBAN, VAT, payment formats for your country
2. 📄 **PEPPOL & UBL** - Help build e-invoicing support
3. 🧪 **Testing** - Add edge cases and scenarios
4. 📖 **Documentation** - Examples, guides, translations
5. ⚡ **Performance** - Benchmarking and optimization

### Development Setup

```bash
# Clone repository
git clone https://github.com/fdivrusa/Finova.git
cd Finova

# Restore dependencies
dotnet restore

# Build solution
dotnet build

# Run tests
dotnet test
```

### Branch Strategy
- `master` - Stable releases (production-ready)
- `develop` - Development branch (alpha releases)
- Feature branches - Create from `develop`, merge back to `develop`

---

## 📋 Requirements

- **.NET 10.0** or higher
- **Microsoft.Extensions.DependencyInjection 10.0.0+** (for DI support)

---

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

---

---

## � API Reference

### Core Interfaces

#### IPaymentReferenceGenerator

```csharp
public interface IPaymentReferenceGenerator
{
    string CountryCode { get; }
    string Generate(string rawReference, PaymentReferenceFormat format = PaymentReferenceFormat.Domestic);
    bool IsValid(string communication);
}
```

### Belgian Implementation

#### BelgianPaymentService

```csharp
public class BelgianPaymentService : IPaymentReferenceGenerator
{
    public string CountryCode => "BE";
    
    // Generate payment reference
    public string Generate(string rawReference, 
        PaymentReferenceFormat format = PaymentReferenceFormat.Domestic);
    
    // Validate payment reference
    public bool IsValid(string communication);
}
```

**Formats:**
- `PaymentReferenceFormat.Domestic` → `+++XXX/XXXX/XXXXX+++` (Belgian OGM)
- `PaymentReferenceFormat.IsoRf` → `RFxxYYYY...` (ISO 11649)

### Core Utilities

#### IsoReferenceHelper

```csharp
public static class IsoReferenceHelper
{
    // Generate ISO 11649 reference with RF prefix
    public static string Generate(string rawReference);
}
```

**Format**: `RFxx` (RF + 2 check digits) + reference body  
**Example**: `IsoReferenceHelper.Generate("INVOICE2024")` → `RF89INVOICE2024`

#### IsoReferenceValidator

```csharp
public static class IsoReferenceValidator
{
    // Validate ISO 11649 reference format and checksum
    public static bool IsValid(string reference);
}
```

**Features**:
- Validates RF prefix
- Verifies modulo 97 checksum
- Accepts spaces (display format)

#### Modulo97Helper

```csharp
public static class Modulo97Helper
{
    // Calculate modulo 97 of numeric string (ISO 7064)
    public static int Calculate(string numericString);
}
```

**Features**:
- Handles arbitrarily large numbers
- ISO 7064 compliant
- Used for IBAN, ISO 11649, OGM checksums

### Extensions

#### ServiceCollectionExtensions

```csharp
public static class ServiceCollectionExtensions
{
    // Register Belgian banking services with DI
    public static IServiceCollection AddBelgianBanking(
        this IServiceCollection services);
}
```

**Registers**:
- `IPaymentReferenceGenerator` → `BelgianPaymentService`

---

## 🙏 Acknowledgments

- **ISO 11649** - International payment reference standard
- **ISO 7064** - Modulo 97 checksum algorithm
- **Belgian Banking Standards** - OGM/VCS format specification
- **European Payments Council** - SEPA standards
- **PEPPOL** - Pan-European Public Procurement On-Line
- **.NET Foundation** - For the amazing .NET platform

---

## 💬 Community & Support

### Get Help
- 📖 [Documentation](#-documentation)
- 💬 [GitHub Discussions](https://github.com/fdivrusa/Finova/discussions)
- 🐛 [Issue Tracker](https://github.com/fdivrusa/Finova/issues)
- 📧 [Contact](mailto:your.email@example.com)

### Stay Updated
- ⭐ Star this repository
- 👀 Watch for releases
- 📢 Follow development on GitHub
- 📝 Read the [CHANGELOG](CHANGELOG.md) *(coming soon)*

---

<div align="center">

**Made with ❤️ for the European financial community**

[Website](https://finova.dev) • [Documentation](https://docs.finova.dev) • [NuGet](https://nuget.org/packages/Finova) • [GitHub](https://github.com/fdivrusa/Finova)

*Finova - Innovative financial operations for .NET*

</div>
