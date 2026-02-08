using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Russia.Validators;

/// <summary>
/// Validator for Russian personal INN (Identifikatsionny Nomer Nalogoplatelshchika).
/// A 12-digit individual tax identification number used as a national identifier.
/// </summary>
public class RussiaNationalIdValidator : INationalIdValidator
{
    public string CountryCode => "RU";

    public ValidationResult Validate(string? input) => RussiaInnValidator.ValidateInn(input);

    public string? Parse(string? input) => Validate(input).IsValid ? input?.Trim() : null;

    public static ValidationResult ValidateStatic(string? input) => new RussiaNationalIdValidator().Validate(input);
}
