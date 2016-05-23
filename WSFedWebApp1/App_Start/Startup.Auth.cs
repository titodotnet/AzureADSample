using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.WsFederation;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
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

            // OMC test
            app.Use((context, next) =>
            {
                PrintCurrentIntegratedPipelineStage(context, "Middleware after set auth type 1");
                return next.Invoke();
            });

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            //app.UseCookieAuthentication(
            //    new CookieAuthenticationOptions
            //    {
            //        AuthenticationType = WsFederationAuthenticationDefaults.AuthenticationType,
            //        Provider = new CookieAuthenticationProvider
            //        {
            //            OnResponseSignIn = crscontext =>
            //            {
            //                crscontext.Identity = TransformClaims(crscontext.Identity);
            //            }
            //        }
            //    });

            // OMC test
            app.Use((context, next) =>
            {
                PrintCurrentIntegratedPipelineStage(context, "Middleware after use cookie auth1");
                return next.Invoke();
            });

            app.UseStageMarker(PipelineStage.PostAuthenticate);
            // OMC test
            app.Use((context, next) =>
            {
                //// Add claim - Begin
                //IAuthenticationManager authenticationManager = context.Authentication;
                //ClaimsIdentity identity = new ClaimsIdentity(HttpContext.Current.User.Identity);
                //identity.AddClaim(new Claim(ClaimTypes.Role, "custom role"));
                //authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = true });
                //// Add claim - End
                PrintCurrentIntegratedPipelineStage(context, "Middleware entry 1");
                return next.Invoke();
            });
            app.UseStageMarker(PipelineStage.PostAuthorize);
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
                            //context.OwinContext.Authentication.AuthenticationResponseGrant
                            //// Add claim - Begin
                            //IAuthenticationManager authenticationManager = context.OwinContext.Authentication;
                            //ClaimsIdentity identity = new ClaimsIdentity();
                            //identity.AddClaim(new Claim(ClaimTypes.Role, "custom role"));
                            //authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = true });
                            //// Add claim - End
                            return Task.FromResult(0);
                        },
                        RedirectToIdentityProvider = context =>
                        {
                            
                            return Task.FromResult(0);
                        },
                        SecurityTokenReceived = context =>
                        {
                            //// Add claim - Begin
                            //IAuthenticationManager authenticationManager = context.OwinContext.Authentication;
                            //ClaimsIdentity identity = new ClaimsIdentity();
                            //identity.AddClaim(new Claim(ClaimTypes.Role, "custom role"));
                            //authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = true });
                            //// Add claim - End
                            var principal = System.Security.Claims.ClaimsPrincipal.Current;
                            return Task.FromResult(0);
                        },
                        SecurityTokenValidated = context =>
                        {
                            //// Add claim - Begin
                            //IAuthenticationManager authenticationManager = context.OwinContext.Authentication;
                            //ClaimsIdentity identity = new ClaimsIdentity();
                            //identity.AddClaim(new Claim(ClaimTypes.Role, "custom role"));
                            //authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = true });
                            //// Add claim - End
                            var principal = System.Security.Claims.ClaimsPrincipal.Current;
                            //var claims = authenticationManager.User.Claims;
                            return Task.FromResult(0);
                        }

                    }
                });

            app.UseClaimsTransformation();

            // OMC test
            app.Use((context, next) =>
            {
                PrintCurrentIntegratedPipelineStage(context, "Middleware 1");
                return next.Invoke();
            });
            //app.Use((context, next) =>
            //{
            //    PrintCurrentIntegratedPipelineStage(context, "2nd MW");
            //    if (context.Authentication.User.Identity.IsAuthenticated)
            //    {
            //        // Add claim - Begin
            //        IAuthenticationManager authenticationManager = context.Authentication;
            //        if (context.Authentication.User.Claims.SingleOrDefault(cl => cl.Type.Equals(ClaimTypes.Role)) == null)
            //        {
            //            ClaimsIdentity identity = new ClaimsIdentity(context.Authentication.User.Identity);
            //            identity.AddClaim(new Claim(ClaimTypes.Role, "custom role"));
            //            authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = true });
            //            ClaimsPrincipal incomingPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
            //            Thread.CurrentPrincipal = new ClaimsPrincipal(identity);
            //            context.Authentication.User = new ClaimsPrincipal(identity);
            //        }
            //        // Add claim - End
            //    }

            //    return next.Invoke();
            //});
        }

        private void PrintCurrentIntegratedPipelineStage(IOwinContext context, string msg)
        {
            var currentIntegratedpipelineStage = HttpContext.Current.CurrentNotification;
            context.Get<TextWriter>("host.TraceOutput").WriteLine(
                "Current IIS event: " + currentIntegratedpipelineStage
                + " Msg: " + msg);
        }

        private ClaimsIdentity TransformClaims(ClaimsIdentity identity)
        {
            if (!identity.Claims.Any(cl=>cl.Type.Equals(ClaimTypes.Role)))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "custom role using tranformation"));
            }

            return identity;
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