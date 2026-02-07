using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.Comoros.Validators;

/// <summary>
/// Validator for Comoros IBANs.
/// Comoros IBAN format: KM + 2 check digits + 23 digits BBAN.
/// Length: 27 characters.
/// </summary>
public class ComorosIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "KM";

    /// <inheritdoc/>
    protected override int ExpectedLength => 27;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => ComorosBbanValidator.Validate(bban);
}
