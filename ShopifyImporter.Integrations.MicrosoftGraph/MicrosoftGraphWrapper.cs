using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ShopifyImporter.Integrations.MicrosoftGraph
{
    public class MicrosoftGraphWrapper
    {
        private string _clientId = "ab7f5eff-4e4e-4d12-b9bc-5c28531bd599";
        private string[] _scopes = new[] { "offline_access", "User.Read", "Files.ReadWrite.All" };

        private IPublicClientApplication _identityClientApp;
        private static GraphServiceClient _graphClient;
        public MicrosoftGraphWrapper()
        {
            _identityClientApp = PublicClientApplicationBuilder.Create(_clientId).WithRedirectUri("http://localhost").Build();
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

        private static async Task<MsalCacheHelper> CreateCacheHelperAsync()
        {
            StorageCreationProperties storageProperties = ConfigureSecureStorage();
            MsalCacheHelper cacheHelper = await MsalCacheHelper.CreateAsync(storageProperties).ConfigureAwait(false);

            return cacheHelper;
        }

        private static StorageCreationProperties ConfigureSecureStorage()
        {
            return new StorageCreationPropertiesBuilder(
                                   "myapp_msal_cache1.txt",
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
