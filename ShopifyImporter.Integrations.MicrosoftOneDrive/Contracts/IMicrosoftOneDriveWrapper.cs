using ShopifyImporter.Integrations.MicrosoftOneDrive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Integrations.MicrosoftOneDrive.Contracts
{
    public interface IMicrosoftOneDriveWrapper
    {
        Task<IEnumerable<string>> ListDriveFolders();
        Task CheckFolderExists(string folderName);
        Task<IEnumerable<string>> DownloadFiles();
        Task UploadFile(string fileName);
        Task DeleteFile(string fileName);
        Task<IEnumerable<DriveModel>> GetDrives();
        Task CreateFolder(string folderName);
    }
}
