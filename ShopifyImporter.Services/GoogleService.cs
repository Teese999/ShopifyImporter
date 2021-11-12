using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using ShopifyImporter.Contracts;
using ShopifyImporter.Integrations.GoogleDrive;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Unity;

namespace ShopifyImporter.Services
{
    public class GoogleService : IGoogleService
    {
        public GoogleApplication _googleApplication = new();
        private IUnityContainer _container;
        public GoogleService(IUnityContainer container)
        {
            _container = container;
            GetApp();
        }

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
        public GoogleApplication GetApp()
        {
            var Scopes = new string[] { DriveService.Scope.Drive };
            

            //get initialize google service

            var googleService = _container.Resolve<IGoogleService>();
            _googleApplication.Credential = googleService.GetCredentialas(Scopes);


            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _googleApplication.Credential,
                ApplicationName = _googleApplication.ApplicationName,
            });
            return _googleApplication;
        }
        public IList<Google.Apis.Drive.v3.Data.File> ListFiles(DriveService service)
        {
            
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 100;
            listRequest.Fields = "nextPageToken, files(id, name, fullFileExtension)";

            //listRequest.PageToken = pageToken;
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;
            _googleApplication.Files = files;

            return files;
        }
    }
}
