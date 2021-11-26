using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShopifyImporter.Contracts;
using ShopifyImporter.Contracts.Models;
using ShopifyImporter.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace ShopifyImporter.Services.Tests
{
    [TestClass()]
    public class ReportServiceTests : AbstractTest
    {
        IReportService _reportService;

        public ReportServiceTests()
        {
            _reportService = _container.Resolve<IReportService>();
        }

        [TestMethod()]
        public void BuildTest()
        {
            var inventories = new List<InventoryDto>();
            inventories.Add(new InventoryDto() { Sku = "001", ErrorMessage = "errorMessage1", HasError = false, Quantity = 1 });
            inventories.Add(new InventoryDto() { Sku = "002", ErrorMessage = "errorMessage2", HasError = false, Quantity = 2 });
            inventories.Add(new InventoryDto() { Sku = "003", ErrorMessage = "errorMessage3", HasError = true, Quantity = 3 });
            inventories.Add(new InventoryDto() { Sku = "004", ErrorMessage = "errorMessage4", HasError = true, Quantity = 4 });
            string fileName = "testFileName";

            var answer = _reportService.Build(inventories, fileName);
            Assert.IsTrue(answer.Length > 0);
        }

        //osipenkom: я думаю, что здесь мы не должны ожидать необработанную ошибку в виде NullReferenceException, а должны ожидать, что тест выполнится успешно.
        //Этот тест сфэйлится и это будет значить, что метод написан с ошибками.
        //Мы должны абстрагироваться от того, что реальный метод уже написан и написать такие тесты, которые по нашему мнению могут сломать метод (вызвать нобработанную ошибку)
        //Ты сделал правильную попытку, что отправил в списке null и ты должен убедиться, что метод эту ситуацию будет в состоянии обработать, но не кинет NullReferenceException
        [TestMethod()]
        public void BuildTest_returned_nullException()
        {
            var inventories = new List<InventoryDto>();
            inventories.Add(new InventoryDto() { Sku = "001", ErrorMessage = "errorMessage1", HasError = false, Quantity = 1 });
            inventories.Add(new InventoryDto() { Sku = "002", ErrorMessage = "errorMessage2", HasError = false, Quantity = 2 });
            inventories.Add(new InventoryDto() { Sku = "003", ErrorMessage = "errorMessage3", HasError = true, Quantity = 3 });
            inventories.Add(new InventoryDto() { Sku = "004", ErrorMessage = "errorMessage4", HasError = true, Quantity = 4 });
            inventories.Add(null);
            string fileName = "testFileName";
            
            Assert.ThrowsException<NullReferenceException>(() => { _reportService.Build(inventories, fileName); });
        }
    }
}