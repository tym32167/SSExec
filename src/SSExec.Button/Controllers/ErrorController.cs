using System.Web.Mvc;
using SSExec.Button.Core;

namespace SSExec.Button.Controllers
{
    public class ErrorController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
    }
}