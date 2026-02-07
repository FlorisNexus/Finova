using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Germany.Validators;

/// <summary>
/// Validator for Germany bank accounts.
/// Germany IBAN format: DE + 2 check digits + 8 bank code + 10 account number (22 characters total).
/// Example : DE89370400440532013000 or formatted: DE89 3704 0044 0532 0130 00
/// </summary>
public class GermanyIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "DE";

    /// <inheritdoc/>
    protected override int ExpectedLength => 22;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => GermanyBbanValidator.Validate(bban);
}
