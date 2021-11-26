﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            _reportService = Container.Resolve<IReportService>();
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

        [TestMethod()]
        public void BuildTest_NullItem()
        {
            var inventories = new List<InventoryDto>();
            inventories.Add(new InventoryDto() { Sku = "001", ErrorMessage = "errorMessage1", HasError = false, Quantity = 1 });
            inventories.Add(new InventoryDto() { Sku = "002", ErrorMessage = "errorMessage2", HasError = false, Quantity = 2 });
            inventories.Add(new InventoryDto() { Sku = "003", ErrorMessage = "errorMessage3", HasError = true, Quantity = 3 });
            inventories.Add(new InventoryDto() { Sku = "004", ErrorMessage = "errorMessage4", HasError = true, Quantity = 4 });
            inventories.Add(null);
            string fileName = "testFileName";
            try
            {
                _reportService.Build(inventories, fileName);
            }
            catch (Exception e)
            {

                Assert.Fail(e.ToString());
            }

        }
        [TestMethod()]
        public void BuildTest_NullCollection()
        {
            string result = null;
            try
            {
                result = _reportService.Build(null, "testFileName");
            }
            catch (Exception e)
            {

                Assert.Fail(e.ToString());
            }
            Assert.AreEqual(result, "Inventories was null");
        }
    }
}