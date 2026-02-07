using Finova.Core.Common;
using Finova.Core.PaymentReference;
using Finova.Validators;
using FluentValidation;

namespace Finova.Extensions.FluentValidation;

public static class PaymentReferenceValidators
{
    /// <summary>
    /// Validates that the string is a valid ISO 11649 Creditor Reference (RF..).
    /// </summary>
    public static IRuleBuilderOptions<T, string?> MustBeValidIsoRf<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder.MustBeValidPaymentReference(PaymentReferenceFormat.IsoRf);
    }

    /// <summary>
    /// Validates that the string is a valid Belgian Structured Message (OGM / VCS).
    /// </summary>
    public static IRuleBuilderOptions<T, string?> MustBeValidOgm<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder.MustBeValidPaymentReference(PaymentReferenceFormat.LocalBelgian);
    }

    /// <summary>
    /// Validates that the string is a valid Swiss QR Reference (ISR).
    /// </summary>
    public static IRuleBuilderOptions<T, string?> MustBeValidSwissQrReference<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder.MustBeValidPaymentReference(PaymentReferenceFormat.LocalSwitzerland);
    }
    /// <summary>
    /// Validates that the string is a valid Payment Reference.
    /// Supports ISO 11649 (RF) and local formats (BE, FI, NO, SE, CH, SI).
    /// </summary>
    /// <param name="format">The expected format (default is IsoRf).</param>
    public static IRuleBuilderOptions<T, string?> MustBeValidPaymentReference<T>(this IRuleBuilder<T, string?> ruleBuilder, PaymentReferenceFormat format = PaymentReferenceFormat.IsoRf)
    {
        return ruleBuilder
            .Must(reference => PaymentReferenceValidator.Validate(reference, format).IsValid)
            .WithMessage(ValidationMessages.InvalidPaymentReference);
    }
}
