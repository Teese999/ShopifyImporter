using ShopifyImporter.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using ShopifyImporter.Contracts;
using System.Linq;
using ShopifyImporter.Console;
using Unity.Lifetime;
using System.IO;

namespace ShopifyImporter.Services.Tests
{
    [TestClass]
    public class ExcelParserServiceTests : AbstractTest
    {

        [TestMethod]
        public void ParseFile_returned_succes()
        {                   
            string filename = @"Schmidts Inventory Report.xlsx";
            int expected = 1808;
            var inventories = Container.Resolve<IExcelParserService>().ParseFile(filename);
            Assert.AreEqual(expected, inventories.Count());
        }
        [TestMethod]
        public void ParseFile_returned_fileNotFoundException()
        {
            string filename = @"Schmidts Inventory Report1.xlsx";

            Assert.ThrowsException<FileNotFoundException>(() => { Container.Resolve<IExcelParserService>().ParseFile(filename); });
        }
    }
}
