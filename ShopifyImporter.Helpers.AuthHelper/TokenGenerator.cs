using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Helpers.AuthHelper
{
    public class TokenGenerator
    {
        private static string[] Scopes = { "Files.ReadWrite.All" };
        private static PublicClientApplication IdentityClientApp;
        public static string TokenForUser = null;
        public static string GetToken(string clientSecret)
        {
            IdentityClientApp = new PublicClientApplication(clientSecret);
            AuthenticationResult authResult;

            authResult = IdentityClientApp.AcquireTokenAsync(Scopes).Result;
            return authResult.Token;

        }
    }
}
