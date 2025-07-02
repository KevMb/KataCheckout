using KataCheckout.Models;
using KataCheckout.Services;
using KataCheckout.Services.Interfaces;
using Moq;
using Xunit.Abstractions;

namespace KataCheckout.Tests
{
    public class PriceServiceTests
    {
        private readonly ITestOutputHelper _output;

        public PriceServiceTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void AddToCartFail()
        {
            var skuService = new Mock<IStockKeepingUnitService>();
            var priceService = new PriceService(skuService.Object);

            Assert.Throws<ArgumentException>(() => priceService.AddToCart(null));
            Assert.Throws<ArgumentException>(() => priceService.AddToCart(""));

            _output.WriteLine("AddToCart failed as expected for null or empty SKU.");
        }

        [Fact]
        public void AddToCartSuccess()
        {
            var skuService = new Mock<IStockKeepingUnitService>();
            var priceService = new PriceService(skuService.Object);

            priceService.AddToCart("A");
            priceService.AddToCart("A");
            priceService.AddToCart("B");

            var cartField = typeof(PriceService).GetField("_cart", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var cart = cartField.GetValue(priceService) as Dictionary<string, int>;

            Assert.Equal(2, cart["A"]);
            Assert.Equal(1, cart["B"]);
            _output.WriteLine($"Cart contents: {string.Join(", ", cart.Select(kvp => $"{kvp.Key}:{kvp.Value}"))}");
        }

        [Fact]
        public void TotalPriceFail()
        {
            var skuService = new Mock<IStockKeepingUnitService>();
            skuService.Setup(s => s.GetAllItems()).Throws(new Exception("SKU data unavailable"));

            var priceService = new PriceService(skuService.Object);
            priceService.AddToCart("A");

            var ex = Assert.Throws<Exception>(() => priceService.TotalPrice());
            _output.WriteLine($"Exception thrown: {ex.Message}");
        }

        [Fact]
        public void TotalPriceSuccess()
        {
            var skuService = new Mock<IStockKeepingUnitService>();
            skuService.Setup(s => s.GetAllItems()).Returns(new List<StockKeepingUnitModel>
            {
                new StockKeepingUnitModel { Sku = "A", UnitPrice = 50 },
                new StockKeepingUnitModel { Sku = "B", UnitPrice = 30 }
            });

            var priceService = new PriceService(skuService.Object);
            priceService.AddToCart("A");
            priceService.AddToCart("A");
            priceService.AddToCart("B");

            var total = priceService.TotalPrice();
            Assert.Equal(130, total); // 50 + 50 + 30
            _output.WriteLine($"Total price calculated: {total}");
        }

        [Fact]
        public void ClearFail()
        {
            var skuService = new Mock<IStockKeepingUnitService>();
            var priceService = new PriceService(skuService.Object);

            var ex = Assert.Throws<NullReferenceException>(() =>
            {
                // Simulate a broken state
                var cartField = typeof(PriceService).GetField("_cart", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                cartField.SetValue(priceService, null);
                priceService.Clear();
            });

            _output.WriteLine($"Exception thrown: {ex.Message}");
        }

        [Fact]
        public void ClearSuccess()
        {
            var skuService = new Mock<IStockKeepingUnitService>();
            var priceService = new PriceService(skuService.Object);

            priceService.AddToCart("A");
            priceService.AddToCart("B");

            priceService.Clear();

            var cartField = typeof(PriceService).GetField("_cart", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var cart = cartField.GetValue(priceService) as Dictionary<string, int>;

            Assert.Empty(cart);
            _output.WriteLine("Cart cleared successfully.");
        }
    }
}
