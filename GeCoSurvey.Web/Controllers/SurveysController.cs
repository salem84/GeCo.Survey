using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeCoSurvey.Service;

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

        public ActionResult Compila()
        {
            var survey = surveyService.GetSurvey(1);
            return View(survey);
        }
    }
}
