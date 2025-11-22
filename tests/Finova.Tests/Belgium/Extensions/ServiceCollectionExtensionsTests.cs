using Finova.Belgium.Extensions;
using Finova.Core.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Finova.Tests.Belgium.Extensions
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddBelgianPaymentReference_RegistersPaymentReferenceGenerator()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddBelgianPaymentReference();
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var service = serviceProvider.GetService<IPaymentReferenceGenerator>();
            service.Should().NotBeNull();
            service!.CountryCode.Should().Be("BE");
        }

        [Fact]
        public void AddBelgianPaymentReference_RegistersServiceAsSingleton()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddBelgianPaymentReference();
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var service1 = serviceProvider.GetService<IPaymentReferenceGenerator>();
            var service2 = serviceProvider.GetService<IPaymentReferenceGenerator>();

            service1.Should().NotBeNull();
            service2.Should().NotBeNull();
            service1.Should().BeSameAs(service2);
        }

        [Fact]
        public void AddBelgianPaymentReference_ReturnsServiceCollection()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = services.AddBelgianPaymentReference();

            // Assert
            result.Should().BeSameAs(services);
        }

        [Fact]
        public void AddBelgianPaymentReference_AllowsMethodChaining()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act & Assert - Should not throw and allow chaining
            Action act = () => services
                .AddBelgianPaymentReference()
                .AddBelgianPaymentReference(); // Can be called multiple times

            act.Should().NotThrow();
        }

        [Fact]
        public void AddBelgianPaymentReference_RegisteredService_CanGenerateReferences()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddBelgianPaymentReference();
            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetRequiredService<IPaymentReferenceGenerator>();

            // Act
            var reference = service.Generate("12345");

            // Assert
            reference.Should().NotBeNullOrWhiteSpace();
            service.IsValid(reference).Should().BeTrue();
        }
    }
}
