﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            _commonService = _container.Resolve<ICommonService>();
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
        public void AuthenticateTest()
        {
            try
            {
                _commonService.Authenticate();
            }
            catch (Exception e)
            {

                Assert.Fail(e.ToString());
            }
        }

    }
}