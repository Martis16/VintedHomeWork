using VintedHomeWork.Models;

namespace VintedHomeWork.Rules
{
    public class DiscountCalculator(IReadOnlyList<IRuleApplier> discountRules)
    {
        private readonly IReadOnlyList<IRuleApplier> _discountRules = discountRules;

        public decimal CalculateDiscount(Shipment shipment, decimal originalPrice)
        {
            if (originalPrice < 0) return 0.0m;
            if (_discountRules.Count <= 0) return 0.0m;

            var length = _discountRules.Count;
            var currentDiscount = 0.0m;
            for (var i = 0; i < length - 1; i++)
            {
                currentDiscount += _discountRules[i].ApplyRule(shipment, originalPrice, 0);
            }
            var discount = _discountRules[length - 1].ApplyRule(shipment, originalPrice, currentDiscount);
            return discount;
        }

    }

}
