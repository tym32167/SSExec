//using Microsoft.Web.WebPages.OAuth;

using System;
using System.Web.Mvc;
using System.Web.Security;
using SSExec.Button.Core;
using SSExec.Web.Models;

namespace SSExec.Button.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        [Authorize(Roles = "Administrator")]
        public ActionResult AddUser()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(LoginModel model)
        {
            if (ModelState.IsValid && Membership.GetUser(model.UserName) == null)
            {
                Membership.CreateUser(model.UserName, model.Password);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Указанные данные некорректны или пользователь с таким логином уже существует.");
            return View(model);
        }



        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (Membership.GetUser("Admin") == null)
            {
                Membership.CreateUser("Admin", "Password");
                Roles.AddUserToRoles("Admin", new[] { "Administrator" });
            }

            if (ModelState.IsValid && Membership.ValidateUser(model.UserName, model.Password))
            {
                FormsAuthentication.RedirectFromLoginPage(model.UserName, false);
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            // WebSecurity.Logout();

            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }




        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion
    }
}
