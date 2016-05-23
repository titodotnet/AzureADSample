using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
//using System.IdentityModel.Claims;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace WSFedWebApp1.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //if (Request.IsAuthenticated)
            //{
            //    // Add claim - Begin
            //    IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            //    ClaimsIdentity identity = new ClaimsIdentity(User.Identity);
            //    identity.AddClaim(new Claim(ClaimTypes.Role, "custom role"));
            //    authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = true });
            //    // Add claim - End
            //}
            return View();
        }

        public ActionResult About()
        {
            var surName = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Surname).Value;
            //throw new Exception("intended ex");
            ViewBag.Message = "Your application description page -." + surName;
            
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}