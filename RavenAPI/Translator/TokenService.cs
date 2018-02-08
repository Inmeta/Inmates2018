using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Translator.Samples
{
    public class TokenService
    {
        private AzureAuthToken _authTokenSource = null;
        private static TokenService _reuse = null;

        private TokenService()
        {
        }
        public static async Task<string> GetTokenAsync()
        {
            if(_reuse is null)
            {
                _reuse = new TokenService()
                {
                    _authTokenSource = new AzureAuthToken("45aa06bbaafa47f4825bec69faf419d9")
                };
            }
            string authToken;
            authToken = await _reuse._authTokenSource.GetAccessTokenAsync();
            return authToken;
        }
    }
}

