using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebGrease;

namespace SSExec.Button.Core
{
    public class LoggingFilter : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)


        {
            // TODO: Add your acction filter's tasks here

            // Log Action Filter Call

            var message = string.Format("{0} {1} {2} {3}",
                HttpContext.Current.User.Identity.IsAuthenticated
                    ? HttpContext.Current.User.Identity.Name
                    : "unknown",
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                filterContext.ActionDescriptor.ActionName,
                filterContext.HttpContext.Request.UserHostAddress);

            new Log().Info(message);

            this.OnActionExecuting(filterContext);
        }
    }
}