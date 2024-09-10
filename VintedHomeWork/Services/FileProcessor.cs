using System.Globalization;
using VintedHomeWork.Models;
using VintedHomeWork.Rules;

namespace VintedHomeWork.Services
{
    public class FileProcessor
    {
        public void ReadAndProcessFile(string path, DiscountCalculator discountCalculator, List<Provider> providersList)
        {
            var lines = File.ReadAllLines(path).Where(x => !string.IsNullOrWhiteSpace(x));
            var sortedLines = lines
                .Where(line => IsValidDate(line.Split(' ')[0]))
                .OrderBy(line => DateTime.ParseExact(line.Split(' ')[0], "yyyy-MM-dd", CultureInfo.InvariantCulture))
                .ToArray();
            foreach (var line in sortedLines)
            {
                ProcessLine(line, discountCalculator, providersList);
            }
        }
        static bool IsValidDate(string dateString)
        {
            DateTime tempDate;
            return DateTime.TryParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out tempDate);
        }


        public void ProcessLine(string line, DiscountCalculator discountCalculator, List<Provider> providers)
        {
            var parts = line.Split(" ");
            if (parts.Length != 3)
            {
                Console.WriteLine($"{line} Ignored");
                return;
            }

            if (!DateTime.TryParse(parts[0], out var date))
            {
                Console.WriteLine($"{line} Ignored");
                return;
            }

            var packageSize = parts[1];
            if (packageSize != "L" && packageSize != "M" && packageSize != "S")
            {
                Console.WriteLine($"{line} Ignored");
                return;
            }

            var carrier = parts[2];
            var provider = providers.Find(p => p.Name == carrier);
            if (provider == null)
            {
                Console.WriteLine($"{line} Ignored");
                return;
            }

            var shipment = new Shipment(date, packageSize, provider);
            ProcessData(discountCalculator, shipment, line);
        }

        public void ProcessData(DiscountCalculator discountCalculator, Shipment shipment, string line)
        {
            try
            {
                var originalPrice = shipment.Provider.GetPrice(shipment.PackageSize);
                if (originalPrice < 0)
                {
                    Console.WriteLine($"{line} Ignored");
                    return;
                }
                var discount = discountCalculator.CalculateDiscount(shipment, originalPrice);
                var finalPrice = originalPrice - discount;
                var discountOutput = discount == 0 ? "-" : discount.ToString();
                Console.WriteLine($"{line} {finalPrice:F2} {discountOutput:F2}");
            }
            catch (AggregateException)
            {
                Console.WriteLine($"{line} Ignored");
            }
        }
    }
}
