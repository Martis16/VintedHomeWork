using VintedHomeWork.Models;

namespace VintedHomeWork.Rules
{
    public class ShipmentSizeSRule(IReadOnlyList<Provider> providers) : IRuleApplier
    {

        public decimal ApplyRule(Shipment shipment, decimal originalPrice, decimal currentDiscount)
        {
            if (originalPrice < 0) { return 0.0m; }
            if (shipment.PackageSize != "S") return 0.0m;
            if (providers.Count < 0) return 0;

            var minPriceAcrossProviders = decimal.MaxValue;
            foreach (var t in providers)
            {
                if (t.GetPrice("S") > 0)
                {
                    minPriceAcrossProviders = Math.Min(minPriceAcrossProviders, t.GetPrice("S"));
                }
            }

            return originalPrice - minPriceAcrossProviders;
        }
    }
}
