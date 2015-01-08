using System.Web.Mvc;
using SSExec.Button.Core;
using SSExec.Button.Models;

namespace SSExec.Button.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly ILog _log;
        private readonly ProcessHelper _processHelper;

        public HomeController(ILog log, ProcessHelper processHelper)
        {
            _log = log;
            _processHelper = processHelper;
        }

        //
        // GET: /Home/

        public ActionResult Index()
        {
            _log.Info(string.Format("{0} call {1}", User.Identity.Name, "Index"));

            var model = new HomeModel();
            model.ProcessInfoViews = _processHelper.GetInfoes();
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Restart(string filename)
        {
            _log.Info(string.Format("{0} call {1} with {2}", User.Identity.Name, "Restart", filename));
            _processHelper.Restart(filename);
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Stop(string filename)
        {
            _log.Info(string.Format("{0} call {1} with {2}", User.Identity.Name, "Stop", filename));
            _processHelper.Kill(filename);
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Start(string filename)
        {
            _log.Info(string.Format("{0} call {1} with {2}", User.Identity.Name, "Start", filename));
            _processHelper.Start(filename);
            return RedirectToAction("Index");
        }

    }
}
