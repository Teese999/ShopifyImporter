using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;
using ShopifyImporter.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace ShopifyImporter.Services
{
    public class GoogleService : IGoogleService
    {
        public UserCredential GetCredentialas(string[] Scopes)
        {
            UserCredential credential;

            using (var stream = new FileStream("appsettings.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

                credPath = Path.Combine(credPath, ".credentials/drive-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            return credential;

        }
        public IList<Google.Apis.Drive.v3.Data.File> ListFiles(DriveService service)
        {
            
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 100;
            listRequest.Fields = "nextPageToken, files(id, name, fullFileExtension)";
            //listRequest.PageToken = pageToken;
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;
            
            return files;
        }
    }
}
