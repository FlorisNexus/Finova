using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Bulgaria.Validators;

/// <summary>
/// Validator for Bulgarian IBANs.
/// </summary>
public class BulgariaIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "BG";

    /// <inheritdoc/>
    protected override int ExpectedLength => 22;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return BulgariaBbanValidator.Validate(bban);
    }
}
