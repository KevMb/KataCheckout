using KataCheckout.Models;
using KataCheckout.Services.Interfaces;

namespace KataCheckout.Services
{
    public class StockKeepingUnitService : IStockKeepingUnitService
    {
        private List<StockKeepingUnitModel> _skus;

        public StockKeepingUnitService()
        {
            _skus =
            [
                new StockKeepingUnitModel
                {
                    Sku = "A",
                    UnitPrice = 50,
                    SpecialPrice = new SpecialPriceModel { Quantity = 3, Price = 130 }
                },
                new StockKeepingUnitModel
                {
                    Sku = "B",
                    UnitPrice = 30,
                    SpecialPrice = new SpecialPriceModel { Quantity = 2, Price = 45 }
                },
                new StockKeepingUnitModel
                {
                    Sku = "C",
                    UnitPrice = 20
                },
                new StockKeepingUnitModel
                {
                    Sku = "D",
                    UnitPrice = 15
                }
            ];
        }

        public IEnumerable<StockKeepingUnitModel> GetAllItems()
        {
            return _skus;
        }

        public string AddNewItem(StockKeepingUnitModel item)
        {
            _skus.Add(item);

            return $"Sku: {item.Sku} was added successfully";
        }

        public string DeleteItem(string sku)
        {
            _skus.RemoveAll(x => x.Sku.Equals(sku, StringComparison.OrdinalIgnoreCase));

            return $"Sku: {sku} was deleted successfully";
        }

        public string DeleteSpecialPriceRule(string sku)
        {
            var item = _skus.FirstOrDefault(x => x.Sku.Equals(sku, StringComparison.OrdinalIgnoreCase));
            if (item is null)
            {
                return $"Sku: {sku} not found";
            }

            if (item.SpecialPrice is null)
            {
                return $"Sku: {sku} does not have a special price rule";
            }

            item.SpecialPrice = null;
            return $"Special price rule for Sku: {sku} was deleted successfully";
        }

        public string UpdateItem(StockKeepingUnitModel item)
        {
            if (item is null || string.IsNullOrWhiteSpace(item.Sku))
            {
                return "Invalid SKU item provided";
            }

            var sku = _skus.FirstOrDefault(x => x.Sku.Equals(item.Sku, StringComparison.OrdinalIgnoreCase));
            if (sku is null)
            {
                return $"Sku: {item.Sku} not found";
            }

            sku.UnitPrice = item.UnitPrice;
            sku.SpecialPrice = item.SpecialPrice;

            return $"Sku: {item.Sku} was updated successfully";
        }
    }
}
