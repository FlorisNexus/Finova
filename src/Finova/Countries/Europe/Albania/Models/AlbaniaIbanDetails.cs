using Finova.Core.Iban;

namespace Finova.Countries.Europe.Albania.Models;

/// <summary>
/// Represents the details of an Albanian IBAN.
/// </summary>
public record AlbaniaIbanDetails : IbanDetails
{
    /// <summary>
    /// Gets the 3-digit bank code.
    /// </summary>
    public required string KodiBankes { get; init; }

    /// <summary>
    /// Gets the 4-digit branch code.
    /// </summary>
    public required string KodiDeges { get; init; }

    /// <summary>
    /// Gets the control character.
    /// </summary>
    public required string ShifraKontrollit { get; init; }

    /// <summary>
    /// Gets the 16-character account number.
    /// </summary>
    public required string NumriLlogarise { get; init; }
}
