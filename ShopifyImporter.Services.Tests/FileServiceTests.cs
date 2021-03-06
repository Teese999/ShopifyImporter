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
            _fileService = Container.Resolve<IFileService>();
        }


        [TestMethod()]
        public async Task DownloadFilesTest()
        {

            var files = await _fileService.DownloadFiles();
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
            var uploadTas = _fileService.UploadFile("test");

            Assert.IsNull(uploadTas.Exception);     
        }

        [TestMethod()]
        public void DeleteFileTest_returned_success()
        {
            var deleteTask = _fileService.DeleteFile("Schmidts Inventory Report22.xlsx");
            Assert.IsNull(deleteTask.Exception);
        }

        [TestMethod()]
        public async Task ListDriveFoldersTest()
        {
            try
            {
                var folders = await _fileService.ListDriveFolders();
                Assert.IsTrue(folders != null);
            }
            catch (Exception e)
            {

                Assert.Fail(e.Message);
            }          
        }

        [TestMethod()]
        public void CheckFoldersTaskExistsTest()
        {
            try
            {
                var folders = _fileService.CheckFolderExists(Settings.Azure.MicrosoftOneDrive.IncomingFolderName);
                System.Console.WriteLine(Settings.Azure.MicrosoftOneDrive.IncomingFolderName);
            }
            catch (Exception e)
            {

                Assert.Fail(e.Message);
            }           
        }

        [TestMethod()]
        public void CreateFolderTest()
        {
            _fileService.CreateFolder("testFolder");
            try
            {
                _fileService.CreateFolder("testFolder");
            }
            catch (Exception e)
            {

                Assert.Fail(e.Message);
            }
        }
    }
}