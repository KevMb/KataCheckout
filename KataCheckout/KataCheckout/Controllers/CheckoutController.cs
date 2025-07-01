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
                Console.WriteLine("1. Checkout");
                Console.WriteLine("2. Add SKU");
                Console.WriteLine("3. Update SKU");
                Console.WriteLine("4. Exit");
                Console.WriteLine("5. Add SKU");
                Console.Write("Select an option: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Checkout();
                        break;
                    case "2":
                        AddSku();
                        break;
                    case "3":
                        UpdateSku();
                        break;
                    case "4":
                        DeleteSku();
                        break;
                    case "5":
                        DeleteSpecialRule();
                        return;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        private void Checkout()
        {

        }

        private void AddSku()
        {
            Console.WriteLine("Enter new SKU details in the format: Sku,UnitPrice,SpecialQuantity,SpecialPrice");
            Console.WriteLine("Example: E,40,3,100 (leave SpecialQuantity and SpecialPrice blank if not applicable, e.g., F,25,,)");
            var input = Console.ReadLine();

            _skuService.AddNewItem(SkuInputCheck(input));
            Console.WriteLine("SKU added successfully.");
        }

        private void UpdateSku()
        {
            Console.WriteLine("Enter SKU details to update in the format: Sku,UnitPrice,SpecialQuantity,SpecialPrice");
            Console.WriteLine("Example: E,40,3,100 (leave SpecialQuantity and SpecialPrice blank if not applicable, e.g., F,25,,)");
            var input = Console.ReadLine();

            _skuService.UpdateItem(SkuInputCheck(input));
            Console.WriteLine("SKU updated successfully.");
        }

        private void DeleteSku()
        {
            Console.Write("Enter the SKU to delete (single letter): ");
            var input = Console.ReadLine();

            var result = _skuService.DeleteItem(SingleLetterCheck(input));
            Console.WriteLine(result);
        }


        private void DeleteSpecialRule()
        {
            Console.Write("Enter the SKU to delete (single letter): ");
            var input = Console.ReadLine();

            var result = _skuService.DeleteSpecialPriceRule(SingleLetterCheck(input));
            Console.WriteLine(result);
        }

        private StockKeepingUnitModel SkuInputCheck(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input cannot be empty.");
                return null;
            }

            var parts = input.Split(',');
            if (parts.Length < 2)
            {
                Console.WriteLine("Invalid input format.");
                return null;
            }

            var sku = parts[0].Trim();
            if (!int.TryParse(parts[1].Trim(), out var unitPrice))
            {
                Console.WriteLine("Invalid unit price.");
                return null;
            }

            var newSkuItem = new StockKeepingUnitModel { Sku = sku, UnitPrice = unitPrice };

            if (parts.Length >= 4 &&
                int.TryParse(parts[2].Trim(), out var specialQuantity) &&
                int.TryParse(parts[3].Trim(), out var specialPriceValue))
            {
                newSkuItem.SpecialPrice = new SpecialPriceModel { Quantity = specialQuantity, Price = specialPriceValue };
            }

            return newSkuItem;
        }

        private string SingleLetterCheck(string? input)
        {
            if (string.IsNullOrWhiteSpace(input) || input.Length != 1 || !char.IsLetter(input[0]))
            {
                return "Input must be a single letter.";
            }
            return input;
        }
    }
}
