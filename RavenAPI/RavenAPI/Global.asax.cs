using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Azure.KeyVault;
using System.Web.Configuration;
using RavenAPI.Helpers;

namespace RavenAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            try
            {
                // I put my GetToken method in a Utils class. Change for wherever you placed your method.
                var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(AuthHelper.GetToken));
                var cosmosConnectionString = kv.GetSecretAsync(WebConfigurationManager.AppSettings["raven-cosmosdb-connectionstring"]).GetAwaiter().GetResult();
                var cosmosKey = kv.GetSecretAsync(WebConfigurationManager.AppSettings["raven-cosmosdb-key"]).GetAwaiter().GetResult();
                //I put a variable in a Utils class to hold the secret for general  application use.
                AuthHelper.CosmosConnectionString = cosmosConnectionString.Value;
                AuthHelper.CosmosKey = cosmosKey.Value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }



        }
    }
}
