using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Italy.Validators;

/// <summary>
/// Validator for Italian IBANs.
/// Italy IBAN format: IT + 2 check + 1 CIN + 5 ABI + 5 CAB + 12 Account (27 characters total).
/// </summary>
public class ItalyIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "IT";

    /// <inheritdoc/>
    protected override int ExpectedLength => 27;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return ItalyBbanValidator.Validate(bban);
    }
}
