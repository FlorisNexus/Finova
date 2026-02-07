using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.Libya.Validators;

/// <summary>
/// Validator for Libya IBANs.
/// Libya IBAN format: LY + 2 check digits + 21 digits BBAN.
/// Length: 25 characters.
/// </summary>
public class LibyaIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "LY";

    /// <inheritdoc/>
    protected override int ExpectedLength => 25;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => LibyaBbanValidator.Validate(bban);
}
