using FluentAssertions;
using VintedHomeWork.Models;
using VintedHomeWork.Rules;

namespace VintedHomeWork.Tests.RulesTests
{
    public class DiscountCalculatorTests
    {
        [Fact]
        public void CalculateDiscount_OriginalPriceIsNegative_ShouldReturnZero()
        {
            // Arrange
            var packageSize = "L";
            var providerName = "LP";
            var prices = new Dictionary<string, decimal>() { { packageSize, 1.5m } };
            var provider = new Provider(providerName, prices);
            var date1 = new DateTime(2024, 8, 4);
            var shipment = new Shipment(date1, packageSize, provider);
            var discountRules = new List<IRuleApplier>();
            var calculator = new DiscountCalculator(discountRules);
            decimal originalPrice = -10.0m;

            // Act
            var discount = calculator.CalculateDiscount(shipment, originalPrice);

            // Assert
            discount.Should().Be(0.0m);
        }

        [Fact]
        public void CalculateDiscount_NoRulesAreProvided_ShouldReturnZero()
        {
            // Arrange
            var discountRules = new List<IRuleApplier>();
            var calculator = new DiscountCalculator(discountRules);
            var packageSize = "L";
            var providerName = "LP";
            var prices = new Dictionary<string, decimal>() { { packageSize, 1.5m } };
            var provider = new Provider(providerName, prices);
            var date1 = new DateTime(2024, 8, 4);
            var shipment = new Shipment(date1, packageSize, provider);
            decimal originalPrice = 100.0m;

            // Act
            var discount = calculator.CalculateDiscount(shipment, originalPrice);

            // Assert
            discount.Should().Be(0.0m);
        }
    }
}
