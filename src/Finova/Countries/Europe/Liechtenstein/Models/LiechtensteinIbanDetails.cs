using Finova.Core.Iban;

namespace Finova.Countries.Europe.Liechtenstein.Models;

/// <summary>
/// Represents the details of a Liechtenstein IBAN.
/// </summary>
public record LiechtensteinIbanDetails : IbanDetails
{
    /// <summary>
    /// Gets the 5-digit bank code.
    /// </summary>
    public required string Bankleitzahl { get; init; }

    /// <summary>
    /// Gets the 12-character account number.
    /// </summary>
    public required string Kontonummer { get; init; }
}
