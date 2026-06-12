using Microsoft.AspNetCore.Mvc;
using ThinknInsurTech.Web.Controllers;

namespace ThinknInsurTech.Web.Public.Controllers
{
    public class HomeController : ThinknInsurTechControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}