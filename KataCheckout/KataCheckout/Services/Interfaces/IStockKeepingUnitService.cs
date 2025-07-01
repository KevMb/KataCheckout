using KataCheckout.Models;

namespace KataCheckout.Services.Interfaces
{
    public interface IStockKeepingUnitService
    {
        IEnumerable<StockKeepingUnitModel> GetAllItems();

        string AddNewItem(StockKeepingUnitModel item);

        string UpdateItem(StockKeepingUnitModel item);

        string DeleteItem(string sku);

        string DeleteSpecialPriceRule(string sku);
    }
}
