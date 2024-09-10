using FluentAssertions;
using VintedHomeWork.Models;
using VintedHomeWork.Rules;

namespace VintedHomeWork.Tests.RulesTests
{
    public class ThirdLShipmentViaLpRuleTests
    {
        [Theory]
        [InlineData("S", "LP", 0.0)]
        [InlineData("M", "LP", 0.0)]
        [InlineData("L", "MR", 0.0)]
        public void ApplyRule_ShouldReturnExpectedDiscount(string packageSize, string providerName, decimal expectedDiscount)
        {
            // Arrange
            var prices = new Dictionary<string, decimal>() { { packageSize, 1.5m } };
            var provider = new Provider(providerName, prices);
            var date = new DateTime(2024, 8, 4);
            var shipment = new Shipment(date, packageSize, provider);
            var rule = new ThirdLShipmentViaLpRule();
            var originalPrice = 0.0m;
            var currentDiscount = 0.0m;

            // Act
            var discount = rule.ApplyRule(shipment, originalPrice, currentDiscount);

            // Assert
            discount.Should().Be(expectedDiscount);
        }
        [Fact]
        public void ApplyRule_OriginalPriceBelowZero_ShouldReturnZero()
        {
            // Arrange
            var packageSize = "L";
            var providerName = "LP";
            var prices = new Dictionary<string, decimal>() { { packageSize, 1.5m } };
            var provider = new Provider(providerName, prices);
            var date = new DateTime(2024, 8, 4);
            var shipment = new Shipment(date, packageSize, provider);
            var rule = new ThirdLShipmentViaLpRule();
            var originalPrice = -1.0m;
            var currentDiscount = 0.0m;

            // Act
            var discount = rule.ApplyRule(shipment, originalPrice, currentDiscount);

            // Assert
            discount.Should().Be(0.0m);
        }

        [Fact]
        public void ApplyRule_ShouldReturnOriginalPrice_ForThirdLShipmentViaLP()
        {
            // Arrange
            var rule = new ThirdLShipmentViaLpRule();
            var packageSize = "L";
            var providerName = "LP";
            var prices = new Dictionary<string, decimal>() { { packageSize, 6.9m } };
            var provider = new Provider(providerName, prices);
            var date1 = new DateTime(2024, 8, 4);
            var date2 = new DateTime(2024, 8, 6);
            var date3 = new DateTime(2024, 8, 8);
            var shipment1 = new Shipment(date1, packageSize, provider);
            var shipment2 = new Shipment(date2, packageSize, provider);
            var shipment3 = new Shipment(date3, packageSize, provider);
            var originalPrice = provider.GetPrice(packageSize);

            // Act
            var result1 = rule.ApplyRule(shipment1, originalPrice, 0m);
            var result2 = rule.ApplyRule(shipment2, originalPrice, 0m);
            var result3 = rule.ApplyRule(shipment3, originalPrice, 0m);

            // Assert
            result1.Should().Be(0m);
            result2.Should().Be(0m);
            result3.Should().Be(originalPrice);
        }


    }
}
