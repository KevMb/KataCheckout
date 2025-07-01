using KataCheckout.Models;
using KataCheckout.Services.Interfaces;
using Moq;
using Xunit.Abstractions;

namespace KataCheckout.Tests
{
    public class StockKeepingUnitServiceTests
    {
        private readonly ITestOutputHelper _output;

        public StockKeepingUnitServiceTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void GetAllSkuItemsFail()
        {
            var skuService = new Mock<IStockKeepingUnitService>();
            skuService.Setup(s => s.GetAllItems()).Throws(new Exception("Failed to retrieve SKU items"));

            var ex = Assert.Throws<Exception>(() => skuService.Object.GetAllItems());
            _output.WriteLine($"Exception thrown: {ex.Message}");

            skuService.Verify(s => s.GetAllItems(), Times.Once);
        }


        [Fact]
        public void GetAllSkuItems()
        {
            var skuService = new Mock<IStockKeepingUnitService>();
            var skus = new List<StockKeepingUnitModel>
            {
                new() { Sku = "A", UnitPrice = 50 },
                new() { Sku = "B", UnitPrice = 30 }
            };

            skuService.Setup(s => s.GetAllItems()).Returns(skus);
            var result = skuService.Object.GetAllItems();

            _output.WriteLine("Returned SKUs:");
            foreach (var sku in result)
            {
                _output.WriteLine($"SKU: {sku.Sku}, UnitPrice: {sku.UnitPrice}");
            }

            Assert.Equal(2, result.Count());
            skuService.Verify(s => s.GetAllItems(), Times.Once);
        }

        [Fact]
        public void AddSkuItemsFail()
        {
            var skuService = new Mock<IStockKeepingUnitService>();
            skuService.Setup(s => s.GetAllItems()).Returns(new List<StockKeepingUnitModel>());
            skuService.Setup(s => s.AddNewItem(It.IsAny<StockKeepingUnitModel>())).Throws(new Exception("Failed to add SKU item"));

            var ex = Assert.Throws<Exception>(() => skuService.Object.AddNewItem(new StockKeepingUnitModel { Sku = "A", UnitPrice = 50 }));
            _output.WriteLine($"Exception thrown: {ex.Message}");

            skuService.Verify(s => s.AddNewItem(It.IsAny<StockKeepingUnitModel>()), Times.Once);
        }

        [Fact]
        public void AddSkuItems()
        {
            var skuService = new Mock<IStockKeepingUnitService>();
            skuService.Setup(s => s.GetAllItems()).Returns(new List<StockKeepingUnitModel>());
            skuService.Setup(s => s.AddNewItem(It.IsAny<StockKeepingUnitModel>()))
                .Returns((StockKeepingUnitModel item) => $"Sku: {item.Sku} was added successfully");

            var newSku = new StockKeepingUnitModel { Sku = "C", UnitPrice = 20 };
            var result = skuService.Object.AddNewItem(newSku);

            _output.WriteLine($"AddNewItem result: {result}");

            skuService.Verify(s => s.AddNewItem(It.IsAny<StockKeepingUnitModel>()), Times.Once);
        }
    }
}