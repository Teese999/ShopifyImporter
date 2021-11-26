using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopifyImporter.Contracts;
using ShopifyImporter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace ShopifyImporter.Services.Tests
{
    [TestClass()]
    public class CommonServiceTests : AbstractTest
    {
        ICommonService _commonService;
        public CommonServiceTests()
        {
            _commonService = Container.Resolve<ICommonService>();
        }

        [TestMethod()]
        public void ExecuteTest()
        {
            try
            {
                _commonService.Execute();
            }
            catch (Exception e)
            {

                Assert.Fail(e.ToString());
            }
        }

        [TestMethod()]
        public async Task CheckFileStorageConfigurationTest()
        {
            (IEnumerable<string>, IEnumerable<string>, IEnumerable<string>) filesResult = new();
            try
            {
                filesResult = await _commonService.CheckFileStorageConfiguration();
                Assert.IsTrue(filesResult.Item1.Any());
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }

        }
    }
}