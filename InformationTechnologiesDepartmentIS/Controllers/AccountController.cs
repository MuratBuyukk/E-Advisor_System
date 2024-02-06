using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using InformationTechnologiesDepartmentIS.Models;

namespace InformationTechnologiesDepartmentIS.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost, ActionName("Register")]
        [AllowAnonymous]
        public ActionResult Register(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                MembershipCreateStatus createStatus;
                MembershipUser newUser = Membership.CreateUser("murat", "12345", "muratbuyuk@example.com", "question", "answer", true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie("murat", true);
                    return RedirectToAction("Index", "Student");
                }
                else
                {
                    ModelState.AddModelError(createStatus.ToString(), "Failed to register.");
                }
            }
            return View(model);
        }
        // GET: Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Student");
            }
            LoginModel model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.Username, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.Username, model.RememberMe);
                    if (Roles.IsUserInRole(model.Username,"Student"))
                    {
                        return RedirectToAction("Index", "Student");
                    }
                    else if (Roles.IsUserInRole(model.Username, "Academician"))
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (Roles.IsUserInRole(model.Username, "Admin"))
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                }
                else
                {
                    ModelState.AddModelError("loginError", "The email or password provided is incorrect!");
                }
            }
            return View(model);
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }
    }
}