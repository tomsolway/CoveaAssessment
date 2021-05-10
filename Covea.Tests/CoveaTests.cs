using Covea.Domain;
using FluentAssertions;
using System;
using Xunit;

namespace Covea.Tests
{
    public class CoveaTests
    {
        [Theory]
        [InlineData(22000)]
        public void Should_ReturnOutOfRangeError(int CalculateInsurancePremium)
        {
            // Arrange
            var sut = new CalculateLifeInsurance();

            // Act
            var result = sut.CalculateInsurancePremium(CalculateInsurancePremium, 0);

            // Assert
            var expected = new Calculated { IsInsuranceCalculated = false };
            result.IsInsuranceCalculated.Should().Be(expected.IsInsuranceCalculated);
        }

        [Theory]
        [InlineData(550000)]
        public void Should_ReturnOutOfRangeError2(int CalculateInsurancePremium)
        {
            // Arrange
            var sut = new CalculateLifeInsurance();

            // Act
            var result = sut.CalculateInsurancePremium(CalculateInsurancePremium, 0);

            // Assert
            var expected = new Calculated { IsInsuranceCalculated = false };
            result.IsInsuranceCalculated.Should().Be(expected.IsInsuranceCalculated);
        }

        [Theory]
        [InlineData(350000)]
        public void Should_ReturnTrue(int CalculateInsurancePremium)
        {
            // Arrange
            var sut = new CalculateLifeInsurance();

            // Act
            var result = sut.CalculateInsurancePremium(CalculateInsurancePremium, 0);

            // Assert
            var expected = new Calculated { IsInsuranceCalculated = true };
            result.IsInsuranceCalculated.Should().Be(expected.IsInsuranceCalculated);
        }

        [Theory]
        [InlineData(25000, 30, 0.43)]
        [InlineData(25000, 31, 2.6075)]
        [InlineData(25000, 55, 6.6925)]
        public void Calculate_RiskPremium(decimal sumAssured, int age, decimal expected)
        {
            // Arrange
            var sut = new CalculateLifeInsurance();

            // Act
            var result = sut.CalculateRiskPremium(sumAssured, age);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(0.43, 0.0129)]
        [InlineData(2.6075, 0.078225)]
        [InlineData(6.6925, 0.200775)]
        public void Calculate_RenewalCommission(decimal riskPremium, decimal expected)
        {
            // Arrange
            var sut = new CalculateLifeInsurance();

            // Act
            var result = sut.CalculateRenewalCommission(riskPremium);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(0.43, 0.0129, 0.4429)]
        [InlineData(2.6075, 0.078225, 2.685725)]
        [InlineData(6.6925, 0.200775, 6.893275)]
        public void Calculate_NetPremium(decimal riskPremium, decimal renewalCommission, decimal expected)
        {
            // Arrange
            var sut = new CalculateLifeInsurance();

            // Act
            var result = sut.CalculateNetPremium(riskPremium, renewalCommission);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(0.4429, 0.907945)]
        [InlineData(2.685725, 5.50573625)]
        [InlineData(6.893275, 14.13121375)]
        public void Calculate_InitialCommission(decimal netPremium, decimal expected)
        {
            // Arrange
            var sut = new CalculateLifeInsurance();

            // Act
            var result = sut.CalculateInitialCommission(netPremium);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(0.4429, 0.907945, 1.350845)]
        [InlineData(2.685725, 5.50573625, 8.19146125)]
        [InlineData(6.893275, 14.13121375, 21.02448875)]
        public void Calculate_GrossPremium(decimal netPremium, decimal initialCommission, decimal expected)
        {
            // Arrange
            var sut = new CalculateLifeInsurance();

            // Act
            var result = sut.CalculateGrossPremium(netPremium, initialCommission);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(30000, 30, 0.5118)]
        public void Calculate_RiskPremiumBetweenBands(decimal sumAssured, int age, decimal expected)
        {
            // Arrange
            var sut = new CalculateLifeInsurance();

            // Act
            var result = sut.CalculateRiskPremium(sumAssured, age);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(25000, 25)]
        public void Should_GrossPremiumGreaterThanTwoPound(decimal sumAssured, int age)
        {
            // Arrange
            var sut = new CalculateLifeInsurance();

            // Act
            var result = sut.CalculateInsurancePremium(sumAssured, age).InsurancePremium;

            // Assert
            result.Should().BeGreaterThan(2);
        }
    }
}
