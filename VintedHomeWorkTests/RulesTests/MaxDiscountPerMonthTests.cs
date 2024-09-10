using FluentAssertions;
using VintedHomeWork.Models;
using VintedHomeWork.Rules;

namespace VintedHomeWork.Tests.RulesTests
{
    public class MaxDiscountPerMonthRuleTests
    {

        [Theory]
        [InlineData(10.0, 10.0)]
        [InlineData(11.0, 10.0)]
        [InlineData(-1.0, 0.0)]
        public void ApplyRule_ShouldAct_Accordingly(decimal testCaseInput, decimal expectedOutput)
        {
            // Arrange
            var rule = new MaxDiscountPerMonthRule();
            var prices = new Dictionary<string, decimal>() { { "S", 1.5m } };
            var name = "LP";
            var provider = new Provider(name, prices);
            var packageSize = "M";
            var date = new DateTime(2024, 8, 4);
            var shipment = new Shipment(date, packageSize, provider);
            prices.TryGetValue("S", out var originalPrice);

            // Act
            var discount = rule.ApplyRule(shipment, originalPrice, testCaseInput);

            // Assert
            discount.Should().Be(expectedOutput);
        }

        [Fact]
        public void ApplyRule_ChangeCurrentDiscount_ShouldReturnHighestPossibleDiscount()
        {
            // Arrange
            var rule = new MaxDiscountPerMonthRule();
            var packageSize = "L";
            var providerName = "LP";
            var prices = new Dictionary<string, decimal>() { { packageSize, 1.5m } };
            var provider = new Provider(providerName, prices);
            var date1 = new DateTime(2024, 8, 4);
            var date2 = new DateTime(2024, 8, 5);
            var shipment1 = new Shipment(date1, packageSize, provider);
            var shipment2 = new Shipment(date2, packageSize, provider);
            var shipment3 = new Shipment(date2, packageSize, provider);
            var originalPrice = 0.0m;
            var currentDiscount1 = 7.0m;
            var currentDiscount2 = 6.0m;
            var currentDiscount3 = 2.0m;

            // Act
            var result1 = rule.ApplyRule(shipment1, originalPrice, currentDiscount1);
            var result2 = rule.ApplyRule(shipment2, originalPrice, currentDiscount2);
            var result3 = rule.ApplyRule(shipment3, originalPrice, currentDiscount3);

            // Assert
            result1.Should().Be(7.0m);
            result2.Should().Be(3.0m);
            result3.Should().Be(0.0m);
        }

    }
}
