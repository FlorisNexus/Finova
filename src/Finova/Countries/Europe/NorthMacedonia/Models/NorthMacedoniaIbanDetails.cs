using Finova.Core.Iban;

namespace Finova.Countries.Europe.NorthMacedonia.Models;

/// <summary>
/// Represents the details of a North Macedonia IBAN.
/// </summary>
public record NorthMacedoniaIbanDetails : IbanDetails
{
    /// <summary>
    /// Gets the 3-digit bank code.
    /// </summary>
    public required string SifraBanka { get; init; }

    /// <summary>
    /// Gets the 10-character account number.
    /// </summary>
    public required string BrojSmetka { get; init; }

    /// <summary>
    /// Gets the 2-digit national check digits.
    /// </summary>
    public required string KontrolenBroj { get; init; }
}
