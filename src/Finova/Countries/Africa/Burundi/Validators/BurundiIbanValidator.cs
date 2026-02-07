using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.Burundi.Validators;

/// <summary>
/// Validator for Burundi IBANs.
/// Burundi IBAN format: BI + 2 check digits + 23 digits BBAN.
/// Length: 27 characters.
/// </summary>
public class BurundiIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "BI";

    /// <inheritdoc/>
    protected override int ExpectedLength => 27;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => BurundiBbanValidator.Validate(bban);
}
