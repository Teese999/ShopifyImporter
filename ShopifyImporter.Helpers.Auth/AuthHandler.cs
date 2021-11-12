using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShopifyImporter.Helpers
{
    public class Authandler : DelegatingHandler
    {
        private IAuthenticationProvider _authenticationProvider;
        public Authandler(IAuthenticationProvider authenticationProvider, HttpMessageHandler innerhandler)
        {
            InnerHandler = innerhandler;
            _authenticationProvider = authenticationProvider;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await _authenticationProvider.AuthenticateRequestAsync(request);
            return await base.SendAsync(request, cancellationToken);
        }

    }
}
