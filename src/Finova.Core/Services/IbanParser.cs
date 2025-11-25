using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using Finova.Core.Models;

namespace Finova.Core.Services
{
    public class IbanParser : IIbanParser
    {
        public string? CountryCode => null;  // Generic service

        /// <summary>
        /// Parses an IBAN into generic details (no country-specific parsing).
        /// For country-specific details, use country-specific service implementations.
        /// </summary>
        public IbanDetails? ParseIban(string? iban)
        {
            if (!IbanHelper.IsValidIban(iban))
            {
                return null;
            }

            var normalized = IbanHelper.NormalizeIban(iban);

            return new IbanDetails
            {
                Iban = normalized,
                CountryCode = normalized[..2],
                CheckDigits = normalized.Substring(2, 2),
                IsValid = true
            };
        }
    }
}
