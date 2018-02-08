using System;
using System.Threading.Tasks;

namespace ProcessMessage
{
    public class TokenService
    {
        private AzureAuthToken _authTokenSource = null;
        private static TokenService _reuse = null;

        private TokenService()
        {
        }
        public static string GetToken()
        {
            if (_reuse is null)
            {
                _reuse = new TokenService()
                {
                    _authTokenSource = new AzureAuthToken("45aa06bbaafa47f4825bec69faf419d9")
                };
            }
            string authToken;
            authToken = _reuse._authTokenSource.GetAccessTokenAsync().GetAwaiter().GetResult();
            return authToken;
        }
    }
}
