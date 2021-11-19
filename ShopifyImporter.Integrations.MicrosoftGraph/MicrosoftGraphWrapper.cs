using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;
using ShopifyImporter.Contracts;
using ShopifyImporter.Integrations.Shopify.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ShopifyImporter.Integrations.MicrosoftGraph
{
    public class MicrosoftGraphWrapper
    {
        private string _clientId;
        private string[] _scopes;
        private string _msalCacheFilename;

        private IPublicClientApplication _identityClientApp;
        private static GraphServiceClient _graphClient;
        public MicrosoftGraphWrapper(Settings settings)
        {
            _clientId = settings.Azure.MicrosoftGraph.AppClientId;
            _scopes = settings.Azure.MicrosoftGraph.Scopes;
            _msalCacheFilename = settings.Azure.MicrosoftGraph.MsalCacheFileName;
            _identityClientApp = PublicClientApplicationBuilder.Create(_clientId).WithRedirectUri(settings.Azure.MicrosoftGraph.AppRedirectUrl).Build();
        }

        public async Task<GraphServiceClient> GetAuthenticatedClient()
        {
            if (_graphClient == null)
            {
                _graphClient = new GraphServiceClient(
                "https://graph.microsoft.com/v1.0",
                new DelegateAuthenticationProvider(
                    async (requestMessage) =>
                    {
                        var token = await GetTokenForUserAsync();
                        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", token.AccessToken);
                        // This header has been added to identify our sample in the Microsoft Graph service.  If extracting this code for your project please remove.
                        requestMessage.Headers.Add("SampleID", "uwp-csharp-apibrowser-sample");

                    }));

                var cacheHelper = await CreateCacheHelperAsync().ConfigureAwait(false);

                cacheHelper.RegisterCache(_identityClientApp.UserTokenCache);
            }

            return _graphClient;
        }

        private async Task<MsalCacheHelper> CreateCacheHelperAsync()
        {
            StorageCreationProperties storageProperties = ConfigureSecureStorage();
            MsalCacheHelper cacheHelper = await MsalCacheHelper.CreateAsync(storageProperties).ConfigureAwait(false);

            return cacheHelper;
        }

        private StorageCreationProperties ConfigureSecureStorage()
        {
            return new StorageCreationPropertiesBuilder(
                                   _msalCacheFilename,
                                   MsalCacheHelper.UserRootDirectory)
                               .Build();

        }

        private async Task<AuthenticationResult> GetTokenForUserAsync()
        {
            AuthenticationResult authResult;

            try
            {
                var accounts = await _identityClientApp.GetAccountsAsync().ConfigureAwait(false);
                var firstAccount = accounts.FirstOrDefault();

                authResult = await _identityClientApp.AcquireTokenSilent(_scopes, firstAccount).ExecuteAsync();
            }
            catch (Exception)
            {
                authResult = await _identityClientApp.AcquireTokenInteractive(_scopes).ExecuteAsync();
            }

            return authResult;
        }
    }
}
