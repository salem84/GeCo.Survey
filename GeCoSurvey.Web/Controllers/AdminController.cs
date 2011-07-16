using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeCoSurvey.Service;
using System.IO;

namespace GeCoSurvey.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly ExcelService excelService;
        private readonly UserService userService;


        public AdminController(ExcelService excelService, UserService userService)
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
                        UserProfile profile = new UserProfile
                        {
                            Matricola = properties[1],
                            Nome = properties[2],
                            Cognome = properties[3],
                            Area = properties[4]
                        };

                        userService.CreaUtente(username, username + "@pavimental.fake", profile, true);
                    }
                }
                
                
                

                
            }

            return View();
        }



       

    }
}
