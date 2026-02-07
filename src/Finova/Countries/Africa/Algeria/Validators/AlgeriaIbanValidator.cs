using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.Algeria.Validators;

/// <summary>
/// Validator for Algeria IBANs.
/// Algeria IBAN format: DZ + 2 check digits + 20 digits (BBAN).
/// Length: 24 characters.
/// </summary>
public class AlgeriaIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "DZ";

    /// <inheritdoc/>
    protected override int ExpectedLength => 24;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => AlgeriaBbanValidator.Validate(bban);
}
