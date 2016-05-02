using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.WsFederation;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WSFedWebApp1
{
    public partial class Startup
    {
        //
        // The Client ID is used by the application to uniquely identify itself to Azure AD.
        // The Metadata Address is used by the application to retrieve the signing keys used by Azure AD.
        // The AAD Instance is the instance of Azure, for example public Azure or Azure China.
        // The Authority is the sign-in URL of the tenant.
        // The Post Logout Redirect Uri is the URL where the user will be redirected after they sign out.
        //
        private static string realm = ConfigurationManager.AppSettings["ida:Wtrealm"];
        private static string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private static string tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        private static string metadata = string.Format("{0}/{1}/federationmetadata/2007-06/federationmetadata.xml", aadInstance, tenant);


        string authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenant);

        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseWsFederationAuthentication(
                new WsFederationAuthenticationOptions
                {                    
                    Wtrealm = realm,
                    MetadataAddress = metadata,
                    Notifications = new WsFederationAuthenticationNotifications
                    {
                        AuthenticationFailed = context =>
                        {
                            context.HandleResponse();
                            context.Response.Redirect("Home/Error?message=" + context.Exception.Message);
                            return Task.FromResult(0);
                        },
                        MessageReceived = context =>
                        {
                            return Task.FromResult(0);
                        },
                        RedirectToIdentityProvider = context =>
                        {
                            
                            return Task.FromResult(0);
                        },
                        SecurityTokenReceived = context =>
                        {
                            var principal = System.Security.Claims.ClaimsPrincipal.Current;
                            return Task.FromResult(0);
                        },
                        SecurityTokenValidated = context =>
                        {
                            var principal = System.Security.Claims.ClaimsPrincipal.Current;
                            return Task.FromResult(0);
                        }

                    }
                });

            app.UseClaimsTransformation();
        }
    }

    public static class AppBuilderExtensions
    {
        public static IAppBuilder UseClaimsTransformation(this IAppBuilder app)
        {
            app.Use(typeof(ClaimsTransformationMiddleware));
            return app;
        }
    }
}