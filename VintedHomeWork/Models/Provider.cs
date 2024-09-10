namespace VintedHomeWork.Models
{
    public class Provider(string name, Dictionary<string, decimal> prices)
    {
        public string Name { get; set; } = name;
        public Dictionary<string, decimal> Prices { get; set; } = prices;

        public decimal GetPrice(string packageSize)
        {
            return Prices[packageSize];
        }
    }
}
