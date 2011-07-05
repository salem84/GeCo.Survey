using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeCoSurvey.Service;
using GeCoSurvey.Domain;

namespace GeCoSurvey.Web.Controllers
{
    public class SurveysController : Controller
    {
        private readonly ISurveyService surveyService;

        public SurveysController(ISurveyService surveyService)
        {
            this.surveyService = surveyService;
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Ciao";

            var surveys = surveyService.GetSurveys(true);

            return View(surveys);
        }

        public ActionResult Compila(int id)
        {
            var survey = surveyService.GetSurvey(id);
            return View(survey);
        }


        [HttpPost]
        public ActionResult Compila(FormCollection formCollection, int? surveyId)
        {
            var form = formCollection.AllKeys.ToDictionary(k => k, v => formCollection[v]);

            return RedirectToAction("Success");
        }


        public ActionResult Success()
        {
            return View();
        }
    }
}
