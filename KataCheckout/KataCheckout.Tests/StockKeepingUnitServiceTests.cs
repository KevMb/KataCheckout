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

        [Fact]
        public void DeleteSkuItemFail()
        {
            var skuService = new Mock<IStockKeepingUnitService>();
            var skuToDelete = "Z";
            var expectedExceptionMessage = "Sku not found";

            skuService.Setup(s => s.DeleteItem(skuToDelete))
                .Throws(new Exception(expectedExceptionMessage));

            var ex = Assert.Throws<Exception>(() => skuService.Object.DeleteItem(skuToDelete));
            _output.WriteLine($"Exception thrown: {ex.Message}");

            skuService.Verify(s => s.DeleteItem(skuToDelete), Times.Once);
        }


        [Fact]
        public void DeleteSkuItem()
        {
            var skuService = new Mock<IStockKeepingUnitService>();
            var skuToDelete = "A";
            var expectedMessage = $"Sku: {skuToDelete} was deleted successfully";

            skuService.Setup(s => s.DeleteItem(skuToDelete)).Returns(expectedMessage);

            var result = skuService.Object.DeleteItem(skuToDelete);

            _output.WriteLine($"DeleteItem result: {result}");

            Assert.Equal(expectedMessage, result);
            skuService.Verify(s => s.DeleteItem(skuToDelete), Times.Once);
        }

        [Fact]
        public void DeleteSpecialPriceRuleFail()
        {
            var skuService = new Mock<IStockKeepingUnitService>();
            var sku = "X";
            var expectedExceptionMessage = "Special price rule not found";

            skuService.Setup(s => s.DeleteSpecialPriceRule(sku))
                .Throws(new Exception(expectedExceptionMessage));

            var ex = Assert.Throws<Exception>(() => skuService.Object.DeleteSpecialPriceRule(sku));
            _output.WriteLine($"Exception thrown: {ex.Message}");

            skuService.Verify(s => s.DeleteSpecialPriceRule(sku), Times.Once);
        }

        [Fact]
        public void DeleteSpecialPriceRuleSuccess()
        {
            var skuService = new Mock<IStockKeepingUnitService>();
            var sku = "A";
            var expectedMessage = $"Special price rule for Sku: {sku} was deleted successfully";

            skuService.Setup(s => s.DeleteSpecialPriceRule(sku)).Returns(expectedMessage);

            var result = skuService.Object.DeleteSpecialPriceRule(sku);
            _output.WriteLine($"DeleteSpecialPriceRule result: {result}");

            Assert.Equal(expectedMessage, result);
            skuService.Verify(s => s.DeleteSpecialPriceRule(sku), Times.Once);
        }

        [Fact]
        public void UpdateItemFail()
        {
            var skuService = new Mock<IStockKeepingUnitService>();
            var itemToUpdate = new StockKeepingUnitModel { Sku = "Z", UnitPrice = 99 };
            var expectedExceptionMessage = "Sku: Z not found";

            skuService.Setup(s => s.UpdateItem(itemToUpdate))
                .Throws(new Exception(expectedExceptionMessage));

            var ex = Assert.Throws<Exception>(() => skuService.Object.UpdateItem(itemToUpdate));
            _output.WriteLine($"Exception thrown: {ex.Message}");

            skuService.Verify(s => s.UpdateItem(itemToUpdate), Times.Once);
        }

        [Fact]
        public void UpdateItemSuccess()
        {
            var skuService = new Mock<IStockKeepingUnitService>();
            var itemToUpdate = new StockKeepingUnitModel
            {
                Sku = "A",
                UnitPrice = 60,
                SpecialPrice = new SpecialPriceModel { Quantity = 2, Price = 100 }
            };
            var expectedMessage = $"Sku: {itemToUpdate.Sku} was updated successfully";

            skuService.Setup(s => s.UpdateItem(itemToUpdate)).Returns(expectedMessage);

            var result = skuService.Object.UpdateItem(itemToUpdate);
            _output.WriteLine($"UpdateItem result: {result}");

            Assert.Equal(expectedMessage, result);
            skuService.Verify(s => s.UpdateItem(itemToUpdate), Times.Once);
        }
    }
}