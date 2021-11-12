using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Microsoft.Graph;

namespace ShopifyImporter.Helpers
{
    public class AuthenticationProvider : IAuthenticationProvider
    {
        private IConfidentialClientApplication _clientApplication;
        private string[] _scopes;

        public AuthenticationProvider(IConfidentialClientApplication clientApplication, string[] scopes)
        {
            _clientApplication = clientApplication;
            _scopes = scopes;
        }

        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            var token = await GetTokenAsync();
            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
        }
        public async Task<string> GetTokenAsync()
        {
            AuthenticationResult authResult = null;

            authResult = await _clientApplication.AcquireTokenForClient(_scopes).ExecuteAsync();

            Console.WriteLine(authResult.AccessToken);

            return authResult.AccessToken;
        }
    }
}
