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
            //osipenkom: можно без .Result
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
        //osipenkom: хороший кейс, но у меня такой тест не работает. потому что у меня в папке на ПК нет такого файла. файл должен лежать прям в папке в тесте,
        //а путь к папке на ПК должен указывать на эту папку
        [TestMethod()]
        public void UploadFileTest_returned_aggregateException()
        {
            var uploadTask = _fileService.UploadFile("Schmidts Inventory Report.xlsx11");
            Assert.IsNotNull(uploadTask.Exception);

        }
        //osipenkom: хороший кейс, но у меня такой тест не работает. потому что у меня в папке на ПК нет такого файла. файл должен лежать прям в папке в тесте,
        //а путь к папке на ПК должен указывать на эту папку
        [TestMethod()]
        public void DeleteFileTest_returned_success()
        {
            var deleteTask = _fileService.DeleteFile("Schmidts Inventory Report22.xlsx");
            Assert.IsNull(deleteTask.Exception);
        }

        [TestMethod()]
        public void ListRootFoldersTest()
        {
            List<string> folders = new();
            try
            {
                //osipenkom: можно без .Result
                folders = _fileService.ListRootFolders().Result.ToList();
            }
            catch (Exception e)
            {

                Assert.Fail(e.Message);
            }

            Assert.IsTrue(folders != null);
        }

        [TestMethod()]
        public void CheckFolderExistsTest()
        {
            try
            {
                //osipenkom: по факту не fodlers, а foldersTask. у меня этот тест не выполняется
                var folders = _fileService.CheckFolderExists(_settings.Azure.MicrosoftOneDrive.IncomingFolderName);
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