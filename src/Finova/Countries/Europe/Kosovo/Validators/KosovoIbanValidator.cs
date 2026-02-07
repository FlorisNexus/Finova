using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Kosovo.Validators;

/// <summary>
/// Validator for Kosovo IBANs.
/// </summary>
public class KosovoIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "XK";

    /// <inheritdoc/>
    protected override int ExpectedLength => 20;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => KosovoBbanValidator.Validate(bban);
}
