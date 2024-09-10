namespace VintedHomeWork.Models
{
    public class Shipment(DateTime date, string packageSize, Provider provider)
    {
        public DateTime Date { get; set; } = date;
        public string PackageSize { get; set; } = packageSize;
        public Provider Provider { get; set; } = provider;
    }
}
