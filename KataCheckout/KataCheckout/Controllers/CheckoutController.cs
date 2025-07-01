using KataCheckout.Models;
using KataCheckout.Services.Interfaces;

namespace KataCheckout.Controller
{
    public class CheckoutController
    {
        private readonly IStockKeepingUnitService _skuService;

        public CheckoutController(IStockKeepingUnitService skuService)
        {
            _skuService = skuService;
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("\nSKU List:");
                foreach (var sku in _skuService.GetAllItems())
                {
                    Console.WriteLine($"SKU: {sku.Sku}, Price: {sku.UnitPrice}, Special: {sku.SpecialPrice?.Quantity} for {sku.SpecialPrice?.Price}");
                }

                Console.WriteLine("\nOptions:");
                Console.WriteLine("1. Add SKU");
                Console.WriteLine("2. Update SKU");
                Console.WriteLine("3. Exit");
                Console.Write("Select an option: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddSku();
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        private void AddSku()
        {
            Console.WriteLine("Enter new SKU details in the format: Sku,UnitPrice,SpecialQuantity,SpecialPrice");
            Console.WriteLine("Example: E,40,3,100 (leave SpecialQuantity and SpecialPrice blank if not applicable, e.g., F,25,,)");
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input cannot be empty.");
                return;
            }

            var parts = input.Split(',');
            if (parts.Length < 2)
            {
                Console.WriteLine("Invalid input format.");
                return;
            }

            var sku = parts[0].Trim();
            if (!int.TryParse(parts[1].Trim(), out var unitPrice))
            {
                Console.WriteLine("Invalid unit price.");
                return;
            }

            var newSkuItem = new StockKeepingUnitModel { Sku = sku, UnitPrice = unitPrice };

            if (parts.Length >= 4 &&
                int.TryParse(parts[2].Trim(), out var specialQuantity) &&
                int.TryParse(parts[3].Trim(), out var specialPriceValue))
            {
                newSkuItem.SpecialPrice = new SpecialPriceModel { Quantity = specialQuantity, Price = specialPriceValue };
            }

            _skuService.AddNewItem(newSkuItem);
            Console.WriteLine("SKU added successfully.");
        }
    }
}
