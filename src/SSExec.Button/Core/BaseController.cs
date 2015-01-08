using System.Web.Mvc;

namespace SSExec.Button.Core
{
    [LoggingFilter]
    [Authorize]
    public abstract class BaseController : Controller
    {
         
    }
}