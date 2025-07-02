using KataCheckout.Services.Interfaces;

namespace KataCheckout.Services
{
    public class PriceService : IPriceService
    {
        private Dictionary<string, int> _cart;

        private IStockKeepingUnitService _stockKeepingUnitService;

        public PriceService(IStockKeepingUnitService stockKeepingUnitService)
        {
            _cart = new Dictionary<string, int>();
            _stockKeepingUnitService = stockKeepingUnitService;
        }

        public void AddToCart(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku))
                throw new ArgumentException("SKU cannot be null or empty.", nameof(sku));

            if (_cart.ContainsKey(sku))
            {
                _cart[sku]++;
            }
            else
            {
                _cart[sku] = 1;
            }
        }

        public int TotalPrice()
        {
            int total = 0;
            var allSkus = _stockKeepingUnitService.GetAllItems().ToList();
            var specialPrices = allSkus.Where(x => x.SpecialPrice is not null).ToList();

            foreach (var item in _cart)
            {
                var sku = allSkus.FirstOrDefault(x => x.Sku.Equals(item.Key, StringComparison.OrdinalIgnoreCase));
                if (sku == null)
                    continue;

                var special = specialPrices.FirstOrDefault(x => x.Sku.Equals(item.Key, StringComparison.OrdinalIgnoreCase))?.SpecialPrice;

                if (special != null && item.Value >= special.Quantity)
                {
                    int specialBundles = item.Value / special.Quantity;
                    int remainder = item.Value % special.Quantity;
                    total += specialBundles * special.Price + remainder * sku.UnitPrice;
                }
                else
                {
                    total += item.Value * sku.UnitPrice;
                }
            }

            return total;
        }

        public void Clear()
        {
            _cart.Clear();
        }
    }
}
