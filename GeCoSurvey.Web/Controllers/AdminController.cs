using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeCoSurvey.Service;

namespace GeCoSurvey.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly ExcelService excelService;


        public AdminController(ExcelService excelService)
        {
            this.excelService = excelService;
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
            excelService.CaricaSurvey();


            return View();
        }






       

    }
}
