using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeCoSurvey.Data;

namespace GeCoSurvey.Web.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles="Administrators, Dipendenti, Responsabili")]
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";
            ViewBag.Username = User.Identity.Name;
            ViewBag.NomeCompleto = ViewBag.Username;

            if (User.IsInRole(Tipologiche.Ruoli.DIPENDENTI))
                ViewBag.Ruolo = Tipologiche.Ruoli.DIPENDENTI;
            else if (User.IsInRole(Tipologiche.Ruoli.RESPONSABILI))
                ViewBag.Ruolo = Tipologiche.Ruoli.RESPONSABILI;

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Admin()
        {
            return View();
        }
    }
}
