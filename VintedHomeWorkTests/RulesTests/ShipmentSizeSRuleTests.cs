using FluentAssertions;
using VintedHomeWork.Models;
using VintedHomeWork.Rules;

namespace VintedHomeWork.Tests.RulesTests
{
    public class ShipmentSizeSRuleTests
    {
        [Theory]
        [InlineData("L", 6.90, 0.0)]
        [InlineData("M", 4.90, 0.0)]
        public void ApplyRule_NonSPackageSize_ShouldReturnZero(string packageSize, decimal price, decimal expectedDiscount)
        {
            // Arrange
            var prices = new Dictionary<string, decimal> { { packageSize, price } };
            var name = "LP";
            var provider = new Provider(name, prices);
            var providers = new List<Provider>() { provider };
            var date = new DateTime(2024, 8, 4);
            var shipment = new Shipment(date, packageSize, provider);
            var rule = new ShipmentSizeSRule(providers);
            var currentDiscount = 0.0m;

            prices.TryGetValue(packageSize, out var originalPrice);

            // Act
            var discount = rule.ApplyRule(shipment, originalPrice, currentDiscount);

            // Assert
            discount.Should().Be(expectedDiscount);
        }

        [Fact]
        public void ApplyRule_PackageSizeS_ShouldReturnDifferenceFromHighestPriceToCheapest()
        {
            // Arrange
            var firstProviderPrices = new Dictionary<string, decimal>() { { "S", 1.50m } };
            var secondProviderPrices = new Dictionary<string, decimal>() { { "S", 1.70m } };
            var thirdProviderPrices = new Dictionary<string, decimal>() { { "S", 1.30m } };
            var forthProviderPrices = new Dictionary<string, decimal>() { { "S", 1.10m } };

            var firstProviderName = "LP";
            var secondProviderName = "MR";
            var thirdProviderName = "NEW";
            var forthProviderName = "Cheapest";

            Provider firstProvider = new(firstProviderName, firstProviderPrices);
            Provider secondProvider = new(secondProviderName, secondProviderPrices);
            Provider thirdProvider = new(thirdProviderName, thirdProviderPrices);
            Provider forthProvider = new(forthProviderName, forthProviderPrices);

            List<Provider> providers = [firstProvider, secondProvider, thirdProvider];
            List<Provider> fourProviders = [firstProvider, secondProvider, thirdProvider, forthProvider];

            var packageSize = "S";
            var date = new DateTime(2024, 8, 4);
            var shipment = new Shipment(date, packageSize, secondProvider);
            var rule = new ShipmentSizeSRule(providers);
            var fourProvidersRule = new ShipmentSizeSRule(fourProviders);
            var currentDiscount = 0.0m;
            secondProviderPrices.TryGetValue("S", out var originalPrice);

            // Act
            var firstDiscount = rule.ApplyRule(shipment, originalPrice, currentDiscount);
            var secondDiscount = fourProvidersRule.ApplyRule(shipment, originalPrice, currentDiscount);

            // Assert
            firstDiscount.Should().Be(0.40m);
            secondDiscount.Should().Be(0.60m);
        }

        [Fact]
        public void ApplyRule_NoProviders_ShouldReturnZero()
        {
            // Arrange
            var prices = new Dictionary<string, decimal>() { };
            var name = "";
            var provider = new Provider(name, prices);
            var packageSize = "M";
            var date = new DateTime(2024, 8, 4);
            var shipment = new Shipment(date, packageSize, provider);
            var providers = new List<Provider>() { };
            var rule = new ShipmentSizeSRule(providers);
            var originalPrice = 0.0m;
            var currentDiscount = 0.0m;

            // Act
            var discount = rule.ApplyRule(shipment, originalPrice, currentDiscount);

            // Assert
            discount.Should().Be(0.0m);
        }

        [Fact]
        public void ApplyRule_OriginalPriceBelowZero_ShouldReturnZero()
        {
            // Arrange
            var prices = new Dictionary<string, decimal>() { { "L", 6.9m } };
            var name = "LP";
            var provider = new Provider(name, prices);
            var packageSize = "L";
            var date = new DateTime(2024, 8, 4);
            var shipment = new Shipment(date, packageSize, provider);
            var providers = new List<Provider>() { };
            var rule = new ShipmentSizeSRule(providers);
            var originalPrice = -1.0m;
            var currentDiscount = 0.0m;

            // Act
            var discount = rule.ApplyRule(shipment, originalPrice, currentDiscount);

            // Assert
            discount.Should().Be(0.0m);
        }
    }
}
