using VintedHomeWork.Models;

namespace VintedHomeWork.Rules
{
    public class ThirdLShipmentViaLpRule : IRuleApplier
    {
        private readonly Dictionary<string, int> _shipmentsDuringMonth = [];

        public decimal ApplyRule(Shipment shipment, decimal originalPrice, decimal currentDiscount)
        {
            if (originalPrice < 0) { return 0.0m; }
            if (shipment.PackageSize != "L" || shipment.Provider.Name != "LP") return 0.0m;

            string yearAndMonth = shipment.Date.Year + shipment.Date.Month.ToString("D2");
            _shipmentsDuringMonth.TryAdd(yearAndMonth, 0);
            _shipmentsDuringMonth[yearAndMonth]++;

            return _shipmentsDuringMonth[yearAndMonth] == 3 ? originalPrice : 0;
        }
    }
}
