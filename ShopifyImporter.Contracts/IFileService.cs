﻿using ShopifyImporter.Contracts.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Contracts
{
    public interface IFileService
    {
        Task<IEnumerable<string>> ListDriveFolders();
        Task CheckFolderExists(string folderName);
        Task CreateFolder(string folderName);
        Task<IEnumerable<string>> DownloadFiles();
        Task UploadFile(string fileName);
        Task DeleteFile(string fileName);
        Task<IEnumerable<DriveDto>> GetDrives();
    }
}
