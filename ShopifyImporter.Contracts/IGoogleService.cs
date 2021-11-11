using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using System;
using System.Collections.Generic;

namespace ShopifyImporter.Contracts
{
    public interface IGoogleService
    {
        UserCredential GetCredentialas(string[] Scopes);
        IList<Google.Apis.Drive.v3.Data.File> ListFiles(DriveService service);
    }
}
