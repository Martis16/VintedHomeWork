//    Vinted homework assignment
//
//    The design pattern I use for rules is strategy pattern:
//
//    *   ShipmentSizeSRule.cs is the first rule - All S shipments should always match the lowest S package price among the providers.
//
//    *   MaxDiscountPerMonthRule.cs is the second rule - Accumulated discounts cannot exceed 10 € in a calendar month.
//        If there are not enough funds to fully cover a discount this calendar month, it should be covered partially.
//
//    *   ThirdLShipmentViaLPRule.cs is the third rule - The third L shipment via LP should be free, but only once a calendar month.
//
//    *   IRuleApplier.cs is an Interface class.
//
//    *   DiscountCalculator.cs is the context class.
//    
//    Using microsoft coding conventions.
//    Using xUnit and fluent assertions for testing
//
//    Assumptions: The first two rules just calculates the discount based on some conditions,
//    while the third rule has to check if the monthly limit isn't reached.
//    So the third rule must be applied last after counting the all possible discounts.
using VintedHomeWork.Models;
using VintedHomeWork.Rules;
using VintedHomeWork.Services;



namespace VintedHomeWork
{
    internal class Program
    {
        private const string PATH = @"..\..\..\input.txt";

        static void Main(string[] args)
        {
            string path;
            if (args.Length == 0)
            {
                path = PATH;
            }
            else
            {
                path = args[0];
            }

            List<Provider> providers = new()
            {
                { new Provider("LP", new Dictionary<string, decimal> { { "S", 1.50m }, { "M", 4.90m }, { "L", 6.90m } }) },
                { new Provider("MR", new Dictionary<string, decimal> { { "S", 2.00m }, { "M", 3.00m }, { "L", 4.00m } }) },

            };

            List<IRuleApplier> discountRules =
            [
                new ShipmentSizeSRule(providers),
                new ThirdLShipmentViaLpRule(),
                new MaxDiscountPerMonthRule()
            ];

            var discountCalculator = new DiscountCalculator(discountRules);

            var fileProcessor = new FileProcessor();
            fileProcessor.ReadAndProcessFile(path, discountCalculator, providers);
        }

    }
}

