using KataCheckout.Models;
using KataCheckout.Services.Interfaces;
using Moq;

namespace KataCheckout.Tests
{
    public class StockKeepingUnitServiceTests
    {
        [Fact]
        public void AddSkuItemsFail()
        {
            var skuService = new Mock<IStockKeepingUnitService>();
            skuService.Setup(s => s.GetAllItems()).Returns(new List<StockKeepingUnitModel>());
            skuService.Setup(s => s.AddNewItem(It.IsAny<StockKeepingUnitModel>())).Throws(new Exception("Failed to add SKU item"));

            Assert.Throws<Exception>(() => skuService.Object.AddNewItem(new StockKeepingUnitModel { Sku = "A", UnitPrice = 50 }));
            skuService.Verify(s => s.AddNewItem(It.IsAny<StockKeepingUnitModel>()), Times.Once);
        }

        [Fact]
        public void AddSkuItems()
        {
            var skuService = new Mock<IStockKeepingUnitService>();
            skuService.Setup(s => s.GetAllItems()).Returns(new List<StockKeepingUnitModel>());
            skuService.Verify(s => s.AddNewItem(It.IsAny<StockKeepingUnitModel>()), Times.Never);
        }
    }
}