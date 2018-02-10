using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;


namespace RavenDatabase.Helpers
{
    public static class AuthHelper
    {
        //this is an optional property to hold the secret after it is retrieved
        public static string EncryptSecret { get; set; }
        public static string CosmosConnectionString { get; set; }
        public static string CosmosKey {
            get
            {
                    // I put my GetToken method in a Utils class. Change for wherever you placed your method.
                    var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(AuthHelper.GetToken));
                    var cosmosConnectionString = kv.GetSecretAsync("https://ravendbkeys.vault.azure.net/secrets/raven-cosmosdb-connectionstring/8d5b00b1614340c0a7c03601899c891a").GetAwaiter().GetResult();
                    var cosmosKey = kv.GetSecretAsync("https://ravendbkeys.vault.azure.net/secrets/raven-cosmosdb-key/783fbe4669404366bf06c083ba9db2be").GetAwaiter().GetResult();
                    //I put a variable in a Utils class to hold the secret for general  application use.
                    AuthHelper.CosmosConnectionString = cosmosConnectionString.Value;
                    //AuthHelper.CosmosKey = cosmosKey.Value;
                return cosmosKey.Value;
            }
            set { }
        }

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