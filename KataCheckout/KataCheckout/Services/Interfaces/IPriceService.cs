namespace KataCheckout.Services.Interfaces
{
    public interface IPriceService
    {
        void AddToCart(string sku);

        int TotalPrice();

        void Clear();
    }
}
