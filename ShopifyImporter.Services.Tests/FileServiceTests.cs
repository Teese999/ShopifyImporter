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
    public class FileServiceTests : AbstractTest
    {
        private IFileService _fileService;
        public FileServiceTests()
        {
            _fileService = _container.Resolve<IFileService>();
        }


        [TestMethod()]
        public void DownloadFilesTest()
        {
            var files = _fileService.DownloadFiles().Result;
            Assert.IsNotNull(files);
            Assert.IsTrue(files.Any());
        }

        [TestMethod()]
        public void UploadFileTest_returned_success()
        {
            var uploadTask = _fileService.UploadFile("Schmidts Inventory Report.xlsx");
            Assert.IsNull(uploadTask.Exception);

        }
        [TestMethod()]
        public void UploadFileTest_returned_aggregateException()
        {
            var uploadTask = _fileService.UploadFile("Schmidts Inventory Report.xlsx11");
            Assert.IsNotNull(uploadTask.Exception);

        }
        [TestMethod()]
        public void DeleteFileTest_returned_success()
        {
            var deleteTask = _fileService.DeleteFile("Schmidts Inventory Report22.xlsx");
            Assert.IsNull(deleteTask.Exception);
        }
    }
}