using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeCoSurvey.Service;
using System.IO;

namespace GeCoSurvey.Web.Areas.Admin.Controllers
{
    //[Authorize(Roles="Administrators")]
    public class LoadController : Controller
    {
        private readonly ExcelService excelService;
        private readonly UserService userService;


        public LoadController(ExcelService excelService, UserService userService)
        {
            this.excelService = excelService;
            this.userService = userService;
        }

        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Utenti()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Utenti(FormCollection collection)
        {
            string username = collection["nome"];
            string area = collection["area"];

            UserProfile profile = userService.GetUtente(username);
            profile.Area = area;
            profile.Save();

            return View();
        }


        [HttpGet]
        public ActionResult CaricaSurvey()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CaricaSurvey(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                excelService.CaricaSurvey(file.InputStream);
            }

            return View();
        }


        [HttpGet]
        public ActionResult CaricaUtenti()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CaricaUtenti(HttpPostedFileBase file)
        {

            if (file != null && file.ContentLength > 0)
            {
                using(var reader = new StreamReader(file.InputStream))
                {
                    while (reader.Peek() >= 0)
                    {
                        string linea = reader.ReadLine();
                        string[] properties = linea.Split(';');

                        string username = properties[0];
                        //utilizzo questo ma probabilmente andrà cambiato
                        Dictionary<string, string> profile = new Dictionary<string, string>();

                        profile.Add("Email", username + "@pavimental.fake");
                        profile.Add("Matricola", properties[1]);
                        profile.Add("Nome", properties[2]);
                        profile.Add("Cognome", properties[3]);
                        profile.Add("Area", properties[4]);
                        
                        userService.CreaUtente(username, profile, true);
                    }
                }
                
                
                

                
            }

            return View();
        }



       

    }
}
