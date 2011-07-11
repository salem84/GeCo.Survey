using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeCoSurvey.Service;
using GeCoSurvey.Domain;
using GeCoSurvey.Web.Models;

namespace GeCoSurvey.Web.Controllers
{
    public class SurveysController : Controller
    {
        private readonly ISurveyService surveyService;
        private readonly IDipendentiService dipendentiService;

        public SurveysController(ISurveyService surveyService,
            IDipendentiService dipendentiService)
        {
            this.surveyService = surveyService;
            this.dipendentiService = dipendentiService;
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

            List<Answer> risposte = new List<Answer>();
            //salva risposte
            foreach (var coppia in form)
            {
                Answer answer = new Answer();
                string strQuestion = coppia.Key.Substring(1); //salto la c
                answer.DomandaId = Convert.ToInt32(strQuestion);
                
                string strSubQuestion = coppia.Value;
                answer.RispostaDataId = Convert.ToInt32(strSubQuestion);
                
                risposte.Add(answer);
            }

            surveyService.SalvaSurvey(User.Identity.Name, risposte);


            return RedirectToAction("Success");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">SurveySessionId</param>
        /// <returns></returns>
        public ActionResult Revisiona(int id)
        {
            SurveyWithAnswers surveyWithAnswers = surveyService.GetSurveyWithAnswers(id);

            
            

            return View(surveyWithAnswers);
        }

        [HttpPost]
        public ActionResult Revisiona(int id, FormCollection formCollection)
        {
            var form = formCollection.AllKeys.ToDictionary(k => k, v => formCollection[v]);

            List<Answer> risposte = new List<Answer>();
            //salva risposte
            foreach (var coppia in form)
            {
                Answer answer = new Answer();
                string strQuestion = coppia.Key.Substring(1); //salto la c
                answer.DomandaId = Convert.ToInt32(strQuestion);

                string strSubQuestion = coppia.Value;
                answer.RispostaDataId = Convert.ToInt32(strSubQuestion);

                risposte.Add(answer);
            }

            surveyService.SalvaSurveyRevisionato(id, risposte);



            return RedirectToAction("Success");
        }

        public ActionResult Success()
        {
            return View();
        }



        public ActionResult Visualizza()
        {
            string responsabile = User.Identity.Name;

            var result = surveyService.GetSurveySessionsByResponsabile(responsabile);

            return View(result);
        }
    }
}
