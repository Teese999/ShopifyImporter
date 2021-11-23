using ShopifyImporter.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using ShopifyImporter.Contracts;
using System.Linq;
using ShopifyImporter.Console;

namespace ShopifyImporter.Services.Tests
{
    [TestClass]
    public class ExcelParserServiceTests
    {

        [TestMethod]
        public void ParseFile_returned_succes()
        {

            string filename = @"Schmidts Inventory Report.xlsx";
            int expected = 1808;
            var excelSrvices = new ExcelParserService(Program.GetSettings()).ParseFile(filename);

            //var actual = excelSrvices.ParseFile(filename);

            Assert.AreEqual(expected, excelSrvices.Count());

        }

    }
}
