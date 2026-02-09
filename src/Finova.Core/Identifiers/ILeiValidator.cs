using Finova.Core.Common;

namespace Finova.Core.Identifiers;

/// <summary>
/// Interface for LEI (Legal Entity Identifier) validation based on ISO 17442.
/// </summary>
public interface ILeiValidator
{
    /// <summary>
    /// Validates a Legal Entity Identifier (LEI).
    /// </summary>
    /// <param name="lei">The LEI string to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    ValidationResult Validate(string? lei);

    /// <summary>
    /// Normalizes the LEI string (trims whitespace and converts to uppercase).
    /// </summary>
    /// <param name="lei">The LEI string.</param>
    /// <returns>The normalized LEI string, or null if input is empty.</returns>
    string? Parse(string? lei);
}
