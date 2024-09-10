using VintedHomeWork.Models;

namespace VintedHomeWork.Rules
{
    public interface IRuleApplier
    {
        public decimal ApplyRule(Shipment shipment, decimal originalPrice, decimal currentDiscount);
    }
}
