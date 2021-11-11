using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using ShopifyImporder.Data.Models;
using ShopifyImporter.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity;
using Unity.Lifetime;

namespace ShopifyImporterConsole
{
    public class Startup
    {
        private IUnityContainer _container = new UnityContainer();

        private  string[] Scopes = { DriveService.Scope.Drive };

        private GoogleApplication _googleApplication = new();

        
        public void StartProgram()
        {
            //Configure container
            ConfigureContainer(_container);

            //get initialize google service
            var googleService = _container.Resolve<IGoogleService>();
            _googleApplication.Credential  = googleService.GetCredentialas(Scopes);


            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _googleApplication.Credential,
                ApplicationName = _googleApplication.ApplicationName,
            });

            _googleApplication.Files = googleService.ListFiles(service);


            
            Console.Read();


        }
        private void ConfigureContainer(IUnityContainer container)
        {
            // Could be used to register more types
            ContainerConfiguration.RegisterTypes<HierarchicalLifetimeManager>(_container);
        }
        

    }
}

