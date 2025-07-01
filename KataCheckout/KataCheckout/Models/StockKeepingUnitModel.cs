namespace KataCheckout.Models
{
    public class StockKeepingUnitModel
    {
        public string Sku { get; set; }
        public int UnitPrice { get; set; }
        public SpecialPriceModel SpecialPrice { get; set; }
    }
}
