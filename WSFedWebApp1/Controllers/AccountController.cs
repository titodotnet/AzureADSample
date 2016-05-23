using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.WsFederation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace WSFedWebApp1.Controllers
{
    public class AccountController : Controller
    {
        public void SignIn()
        {
            // Send a WSFederation sign-in request.
            if (!Request.IsAuthenticated)
            {
                //// Add claim - Begin
                //IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                //ClaimsIdentity identity = new ClaimsIdentity(User.Identity);
                //identity.AddClaim(new Claim(ClaimTypes.Role, "custom role"));
                //authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = true });
                //// Add claim - End
                HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/" }, WsFederationAuthenticationDefaults.AuthenticationType);
                //// Add claim - Begin
                //IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                //ClaimsIdentity identity = new ClaimsIdentity(User.Identity);
                //identity.AddClaim(new Claim(ClaimTypes.Role, "custom role"));
                //authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = true });
                //// Add claim - End
            }
        }
        public void SignOut()
        {
            // Send a WSFederation sign-out request.
            HttpContext.GetOwinContext().Authentication.SignOut(
                WsFederationAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
        }
    }
}