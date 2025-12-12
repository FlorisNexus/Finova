# Contributing to Finova

Thank you for your interest in contributing to Finova! We welcome contributions from the community to help make this the best offline financial validation toolkit for .NET.

## 🤝 How to Contribute

### 1. Reporting Bugs
If you find a bug, please open an issue on GitHub. Include:
- A clear description of the issue.
- A minimal reproduction code snippet.
- The expected behavior vs. the actual behavior.

### 2. Suggesting Features
We love new ideas! Please open an issue to discuss your feature request before implementing it. This ensures it aligns with the project's goals (specifically: **100% offline**, **zero dependencies**).

### 3. Pull Requests
1.  **Fork** the repository.
2.  Create a **feature branch** (`git checkout -b feature/my-new-feature`).
3.  Commit your changes following the **Conventional Commits** standard (e.g., `feat: add Italy IBAN validation`).
4.  **Test** your changes (`dotnet test`).
5.  Push to your branch and open a **Pull Request**.

---

## 💻 Coding Standards

We enforce strict coding standards to ensure maintainability and consistency.

### General Rules
- **Language:** C# (Latest supported version).
- **Frameworks:** .NET Standard 2.0 / .NET 8.0+.
- **Dependencies:** **ZERO** external dependencies (except for testing and `FluentValidation` in the extensions package).

### Code Style
- Use **PascalCase** for classes and methods.
- Use **camelCase** for local variables and parameters.
- Always use **curly braces** `{ }` for control flow statements (`if`, `for`, `foreach`), even for single lines.
- **Document** all public members using XML documentation (`///`).

### Testing
- **Mandatory:** Every new feature or bug fix **MUST** include unit tests.
- **Coverage:** We aim for high code coverage (> 80%).
- **Framework:** xUnit + FluentAssertions.

---

## 🏗️ Project Structure

- `src/Finova`: Main library containing country-specific implementations.
- `src/Finova.Core`: Shared interfaces and utilities (Modulo97, Luhn, etc.).
- `src/Finova.Extensions.FluentValidation`: FluentValidation integration.
- `tests/Finova.Tests`: Unit tests for all projects.

## 🌍 Adding a New Country

1.  Create a folder in `src/Finova/Countries/Region/CountryName`.
2.  Implement the necessary validators (IBAN, VAT, etc.).
3.  Add unit tests in `tests/Finova.Tests/Countries/Region/CountryName`.
4.  Ensure all tests pass.

Thank you for contributing! 🚀
