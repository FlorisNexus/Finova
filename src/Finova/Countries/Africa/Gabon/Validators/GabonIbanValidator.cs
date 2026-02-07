using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.Gabon.Validators;

/// <summary>
/// Validator for Gabon IBANs.
/// Gabon IBAN format: GA + 2 check digits + 23 digits BBAN.
/// Length: 27 characters.
/// </summary>
public class GabonIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "GA";

    /// <inheritdoc/>
    protected override int ExpectedLength => 27;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => GabonBbanValidator.Validate(bban);
}
