using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.UnitedKingdom.Validators;

/// <summary>
/// Validator for United Kingdom bank accounts.
/// United Kingdom IBAN format: GB + 2 check digits + 4 bank code + 6 sort code + 8 account number (22 characters total).
/// Example: GB29NWBK60161331926819 or formatted: GB29 NWBK 6016 1331 9268 19
/// </summary>
public class UnitedKingdomIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "GB";

    /// <inheritdoc/>
    protected override int ExpectedLength => 22;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => UnitedKingdomBbanValidator.Validate(bban);
}
