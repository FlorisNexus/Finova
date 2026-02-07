using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Belgium.Validators;

/// <summary>
/// Validator for Belgian IBAN bank accounts.
/// Belgian IBAN format: BE + 2 check digits + 3 Bank + 7 Account + 2 National Check.
/// Total: 16 characters.
/// </summary>
public class BelgiumIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "BE";

    /// <inheritdoc/>
    protected override int ExpectedLength => 16;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => BelgiumBbanValidator.Validate(bban);
}
