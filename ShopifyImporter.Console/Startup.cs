using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using ShopifyImporder.Data.Models;
using ShopifyImporter.Contracts;
using ShopifyImporter.Helpers;
using ShopifyImporter.Integrations.GoogleDrive;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity;
using Unity.Lifetime;

namespace ShopifyImporter.Console
{
    public class Startup
    {
        private IUnityContainer _container = new UnityContainer();
        private static GraphServiceClient _graphClient;

        public void StartProgram()
        {
            //Configure container
            ConfigureContainer(_container);
            
            var config = LoadAppSettings();
            var client = GetAuthenticatedGraphClient(config);
            var graphRequest = client.Drives.Request();

            var results = graphRequest.GetAsync().Result;

            //foreach (var item in results)
            //{
            //    System.Console.WriteLine(item.ToString());
            //}
            System.Console.Read();
        }
        private void ConfigureContainer(IUnityContainer container)
        {
            // Could be used to register more types
            ContainerConfiguration.RegisterTypes<HierarchicalLifetimeManager>(_container);
        }

        private static IConfigurationRoot LoadAppSettings()
        {

            var config = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();
            return config;
        }
        private static IAuthenticationProvider CreateAutorizationProvider(IConfigurationRoot config)
        {
            var clientId = config["applicationId"];
            var clientSecret = config["applicationSecret"];
            var redirectUri = config["redirectUri"];
            var tenant = config["tenant"];
            var authority = $"https://login.microsoftonline.com/{tenant}/.well-known/openid-configuration";
            List<string> scopes = new();
            scopes.Add("https://graph.microsoft.com/.default");
            var cca = ConfidentialClientApplicationBuilder.Create(clientId)
                //.WithAdfsAuthority(authority)
                .WithTenantId(tenant)
                .WithRedirectUri(redirectUri)
                .WithClientSecret(clientSecret)
                .Build();
            return new AuthenticationProvider(cca, scopes.ToArray());
        }

        private static GraphServiceClient GetAuthenticatedGraphClient(IConfigurationRoot config)
        {
            var authenticationProvider = CreateAutorizationProvider(config);
            _graphClient = new GraphServiceClient(authenticationProvider);
            return _graphClient;
        }
    }
}

