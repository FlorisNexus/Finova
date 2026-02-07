using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.Congo.Validators;

/// <summary>
/// Validator for Congo IBANs.
/// Congo IBAN format: CG + 2 check digits + 23 digits BBAN.
/// Length: 27 characters.
/// </summary>
public class CongoIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "CG";

    /// <inheritdoc/>
    protected override int ExpectedLength => 27;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => CongoBbanValidator.Validate(bban);
}
