using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace OpenIdConnectWebApp1.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            string name = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Name).Value;
            string upn = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Upn).Value;

            TempData.Add("claimname", name);
            TempData.Add("claimupn", upn);

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}