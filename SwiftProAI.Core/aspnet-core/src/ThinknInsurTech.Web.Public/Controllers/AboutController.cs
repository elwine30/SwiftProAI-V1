using Microsoft.AspNetCore.Mvc;
using ThinknInsurTech.Web.Controllers;

namespace ThinknInsurTech.Web.Public.Controllers
{
    public class AboutController : ThinknInsurTechControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}