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
            throw new NotImplementedException();
        }

        public string DeleteSpecialPriceRule(string sku)
        {
            throw new NotImplementedException();
        }

        public string UpdateItem(StockKeepingUnitModel item)
        {
            throw new NotImplementedException();
        }
    }
}
