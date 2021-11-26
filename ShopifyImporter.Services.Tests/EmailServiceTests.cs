using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopifyImporter.Console;
using ShopifyImporter.Contracts;
using ShopifyImporter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Lifetime;

namespace ShopifyImporter.Services.Tests
{
    [TestClass()]
    public class EmailServiceTests : AbstractTest
    {
        private IEmailService _emailService;
        public EmailServiceTests()
        {
            _emailService = Container.Resolve<IEmailService>();
        }

        [TestMethod()]
        public void SendTest_returned_succes()
        {
            try
            {
                _emailService.Send(Settings.Smtp.EmailFrom, Settings.Smtp.EmailTo, "test", "test");
            }
            catch (Exception e)
            {

                Assert.Fail(e.ToString());
            }           
        }
        [TestMethod()]
        public void SendTest_returned_formatException()
        {
            Assert.ThrowsException<FormatException>(() => { _emailService.Send("test", "test", "test", "test"); });
        }
    }
}