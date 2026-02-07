using Finova.Core.Iban;

namespace Finova.Countries.Europe.Montenegro.Models;

/// <summary>
/// Represents the details of a Montenegro IBAN.
/// </summary>
public record MontenegroIbanDetails : IbanDetails
{
    /// <summary>
    /// Gets the 3-digit bank code.
    /// </summary>
    public required string SifraBanke { get; init; }

    /// <summary>
    /// Gets the 13-character account number.
    /// </summary>
    public required string BrojRacuna { get; init; }

    /// <summary>
    /// Gets the 2-digit national check digits.
    /// </summary>
    public required string KontrolniBroj { get; init; }
}
