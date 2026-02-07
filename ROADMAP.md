# Finova NuGet Package Roadmap

## Overview
This document outlines features for the Finova NuGet packages (Finova.Core, Finova, Finova.Extensions). These are core validation library enhancements and SDK ports.

---

## Priority 1: Critical for Market Viability

---

### 1.4 JavaScript/TypeScript SDK (npm)

**Why**: JavaScript is the world's most used language. Essential for frontend and Node.js adoption.

**Scope**:
- Port core validators to TypeScript
- Publish to npm as `finova` or `@finova/validator`
- Tree-shakeable ES modules
- Browser and Node.js compatible

**Master Prompt**:
```
Create a JavaScript/TypeScript SDK for Finova and publish to npm.

Requirements:
1. Create a new directory: sdk/javascript
2. Port core validation logic from Finova.Core to TypeScript:
   - IBAN validation (Mod97)
   - Payment card validation (Luhn algorithm)
   - VAT format validation (all countries)
   - BIC validation
   - National ID validation (major countries)
3. Structure as tree-shakeable ES modules
4. Support both ESM and CommonJS
5. Zero runtime dependencies
6. Include TypeScript type definitions
7. Add comprehensive JSDoc comments
8. Create package.json with proper metadata, keywords, repository links
9. Add README with usage examples
10. Set up build with tsup or esbuild
11. Include unit tests (Jest or Vitest)

Match the API style of the .NET version where possible. Target npm package name: @finova/validator
```

---

### 1.5 Python SDK (PyPI)

**Why**: Python dominates data processing, fintech backends, and automation scripts.

**Scope**:
- Port core validators to Python
- Publish to PyPI as `finova`
- Type hints for IDE support
- Async support for batch operations

**Master Prompt**:
```
Create a Python SDK for Finova and publish to PyPI.

Requirements:
1. Create a new directory: sdk/python
2. Port core validation logic from Finova.Core to Python:
   - IBAN validation (Mod97)
   - Payment card validation (Luhn algorithm)
   - VAT format validation (all countries)
   - BIC validation
   - National ID validation (major countries)
3. Use modern Python (3.9+) with type hints throughout
4. Structure as a proper Python package with __init__.py exports
5. Zero required dependencies (optional: pydantic for models)
6. Include comprehensive docstrings
7. Create pyproject.toml with proper metadata
8. Add README with usage examples
9. Include unit tests (pytest)
10. Support both sync and async validation for batch operations

Match the API style of the .NET version. Target PyPI package name: finova
```

---

## Priority 2: High Value Additions

---

### 2.3 Intelligent Error Messages with Suggestions

**Why**: Improves UX dramatically - helps users fix mistakes instead of just rejecting input.

**Scope**:
- Detect common typos (O/0, I/1, transpositions)
- Suggest corrections
- Calculate similarity scores

**Master Prompt**:
```
Implement intelligent error messages with correction suggestions for Finova validators.

Requirements:
1. Create an ErrorSuggestionService that analyzes invalid inputs
2. Detect common mistake patterns:
   - Character substitutions: O<->0, I<->1, S<->5, B<->8
   - Adjacent transpositions: "BE68" vs "BE86"
   - Missing/extra characters
   - Wrong check digit (calculate correct one)
3. Enhance ValidationResult to include:
   - suggestions: string[] (up to 3 likely corrections)
   - suggestion_confidence: number (0-1)
   - error_type: "check_digit" | "length" | "format" | "typo"
4. For IBAN: If only check digit is wrong, calculate and suggest correct IBAN
5. For VAT: Suggest correct format with examples
6. Apply to all validators: IBAN, VAT, payment cards, national IDs
7. Add unit tests with common mistake scenarios

Example output:
{
  "is_valid": false,
  "error": "Invalid check digit",
  "suggestions": ["BE68539007547034"],
  "suggestion_confidence": 0.95
}
```

---

## Priority 3: Market Expansion (Core Enhancements)

These enhance the core validation library with additional country support and bank lookups.

---

### 3.1 US ABA Routing Number Validation Enhancement

**Why**: US is the largest market - need proper checksum validation in core library.

**Scope**:
- ABA routing number checksum algorithm
- Fedwire vs ACH distinction

**Master Prompt**:
```
Enhance US routing number validation in Finova.Core.

Requirements:
1. Implement ABA routing number checksum validation:
   - 9 digits with weighted checksum (3-7-1-3-7-1-3-7-1)
   - Checksum: (3*d1 + 7*d2 + 1*d3 + 3*d4 + 7*d5 + 1*d6 + 3*d7 + 7*d8 + 1*d9) mod 10 = 0
2. Create UsRoutingValidator in Finova.Core
3. Return UsRoutingDetails with:
   - RoutingNumber (normalized)
   - FedReserveDistrict (first 2 digits indicate district)
   - IsValid
4. Add to ChecksumHelper if reusable
5. Include comprehensive unit tests

This is format validation only - bank lookup is in Finova-site.
```

---

### 3.2 India IFSC Format Validation

**Why**: India is a massive market with unique payment infrastructure.

**Scope**:
- IFSC code format validation in core library

**Master Prompt**:
```
Add India IFSC code format validation to Finova.Core.

Requirements:
1. Create IndiaIfscValidator:
   - Format: 11 characters (4 bank code + 0 + 6 branch code)
   - First 4 chars: alphabetic bank code
   - 5th char: always 0 (reserved)
   - Last 6 chars: alphanumeric branch code
2. Return IndiaIfscDetails with:
   - BankCode (first 4 chars)
   - BranchCode (last 6 chars)
   - IsValid
3. Add known bank code prefixes for major banks (SBI, HDFC, ICICI, etc.)
4. Include unit tests with real IFSC examples

This is format validation only - bank/branch lookup is in Finova-site.
```

---

### 3.3 Australian BSB Format Validation

**Why**: Australia uses BSB codes - add to core library.

**Scope**:
- BSB format validation

**Master Prompt**:
```
Add Australian BSB format validation to Finova.Core.

Requirements:
1. Create AustraliaBsbValidator:
   - Format: 6 digits (XXX-XXX or XXXXXX)
   - First 2 digits: bank code
   - 3rd digit: state code
   - Last 3 digits: branch code
2. Return AustraliaBsbDetails with:
   - BankCode
   - StateCode
   - BranchCode
   - State (NSW, VIC, QLD, SA, WA, TAS, NT, ACT based on digit)
   - IsValid
3. Validate state code is in valid range (2-7)
4. Include unit tests

This is format validation only - bank lookup is in Finova-site.
```

---

### 3.4 Canadian Transit Number Format Validation

**Why**: Canada uses transit numbers - complete North American coverage.

**Scope**:
- Transit number format validation

**Master Prompt**:
```
Add Canadian transit number format validation to Finova.Core.

Requirements:
1. Create CanadaTransitValidator:
   - Format 1: XXXXX-YYY (5-digit transit + 3-digit institution)
   - Format 2: 0YYYXXXXX (EFT format: leading 0 + institution + transit)
2. Return CanadaTransitDetails with:
   - TransitNumber (5 digits)
   - InstitutionNumber (3 digits)
   - IsValid
   - Format (Standard/EFT)
3. Validate institution number is in known range (001-999)
4. Include unit tests with examples from major banks

This is format validation only - bank lookup is in Finova-site.
```

---

## Execution Instructions

**Recommended execution order:**
1. Start with SDKs (1.4, 1.5) - can be done in parallel
2. Add intelligent error messages (2.3) - enhances all validators
3. Add country-specific validators (3.1-3.4) as needed for market expansion

**Before each implementation:**
- Run existing tests to confirm stability
- Create a feature branch

**After each implementation:**
- Run all tests
- Update NuGet package version
- Update CHANGELOG.md

---

## Dependencies Map

```
JS SDK (1.4) ---------> Independent
Python SDK (1.5) -----> Independent
Error Suggestions (2.3) -> Enhances existing ValidationResult
US Routing (3.1) -----> New validator
India IFSC (3.2) -----> New validator
Australia BSB (3.3) --> New validator
Canada Transit (3.4) -> New validator
```

---

## Success Metrics

After implementing Priority 1:
- [ ] Available on npm as @finova/validator
- [ ] Available on PyPI as finova
- [ ] Both SDKs have feature parity with core .NET validators

After implementing Priority 2-3:
- [ ] Error suggestions improve user experience
- [ ] Core validators cover major markets (US, India, Australia, Canada)

---

*Last updated: 2026-01-31*
