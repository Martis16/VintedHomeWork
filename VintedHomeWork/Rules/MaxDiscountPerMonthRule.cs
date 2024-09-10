using VintedHomeWork.Models;

namespace VintedHomeWork.Rules
{
    public class MaxDiscountPerMonthRule : IRuleApplier
    {
        private const decimal MaxMonthlyDiscount = 10.0m;
        private readonly Dictionary<string, decimal> _accumulatedDiscounts = [];

        public decimal ApplyRule(Shipment shipment, decimal originalPrice, decimal currentDiscount)
        {
            if (currentDiscount < 0) { return 0.0m; }

            decimal discount;
            string yearAndMonth = shipment.Date.Year + shipment.Date.Month.ToString("D2");

            _accumulatedDiscounts.TryAdd(yearAndMonth, 0.0m);

            if (_accumulatedDiscounts[yearAndMonth] + currentDiscount > MaxMonthlyDiscount)
            {
                discount = MaxMonthlyDiscount - _accumulatedDiscounts[yearAndMonth];
                _accumulatedDiscounts[yearAndMonth] = MaxMonthlyDiscount;
            }
            else
            {
                discount = currentDiscount;
                _accumulatedDiscounts[yearAndMonth] += discount;
            }
            return discount;
        }
    }
}
