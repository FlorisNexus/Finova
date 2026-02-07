using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Ireland.Validators;

/// <summary>
/// Validator for Irish IBANs.
/// Ireland IBAN format: IE + 2 check + 4 Bank Code + 6 Sort Code + 8 Account (22 characters total).
/// </summary>
public class IrelandIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "IE";

    /// <inheritdoc/>
    protected override int ExpectedLength => 22;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return IrelandBbanValidator.Validate(bban);
    }
}
