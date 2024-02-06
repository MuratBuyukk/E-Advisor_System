using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace InformationTechnologiesDepartmentIS.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            MembershipCreateStatus status;

            //Membership.CreateUser("gulsum@com", "12345", "gulsum@com", "question", "answer", true, out status);
            //Roles.AddUserToRole("gulsum@com", "Academician");
            return View();
        }
    }
}