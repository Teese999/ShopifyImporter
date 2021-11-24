using Microsoft.Graph;
using Microsoft.Identity.Client.Extensions.Msal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Integrations.MicrosoftGraph.Contracts
{
    public interface IMicrosoftGraphWrapper
    {
        Task<GraphServiceClient> GetAuthenticatedClient();


    }
}
