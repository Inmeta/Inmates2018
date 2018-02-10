using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Threading.Tasks;


namespace RavenDatabase.Helpers
{
    public static class AuthHelper
    {
        //this is an optional property to hold the secret after it is retrieved
        public static string EncryptSecret { get; set; }
        public static string CosmosConnectionString { get; set; }
        public static string CosmosKey { get; set; }

        //the method that will be provided to the KeyVaultClient
        public static async Task<string> GetToken(string authority, string resource, string scope)
        {
            var authContext = new AuthenticationContext(authority);
            ClientCredential clientCred = new ClientCredential(
                "7b81e2a4-2637-4a8f-ba01-2c5ec382d4e8",
                "SSm55d7hBpiwSXtdrzIFip0H9HtkMxcigaOGXfB5aAI=");
            AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCred);

            if (result == null)
                throw new InvalidOperationException("Failed to obtain the JWT token");

            return result.AccessToken;
        }
    }
}