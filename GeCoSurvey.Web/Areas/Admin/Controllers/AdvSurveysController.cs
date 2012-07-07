using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeCoSurvey.Domain;
using GeCoSurvey.Service;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace GeCoSurvey.Web.Areas.Admin.Controllers
{
    public class AdvSurveysController : Controller
    {
        private readonly ISurveyService surveyService;
        private readonly LogWriter logger;

        public AdvSurveysController(ISurveyService surveyService,
            IDipendentiService dipendentiService,
            LogWriter logger
            )
        {
            this.surveyService = surveyService;
            this.logger = logger;

        }


        /// <summary>
        /// Visualizza i questionari e da la possibilita di eliminarli
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            string username = User.Identity.Name;

            logger.Write(string.Format("L'utente {0} ", username));

            //Segno per ogni survey, quali sono stati già compilati dall'utente
            var surveys = surveyService.GetAllSurveySession();

            return View(surveys);
        }

        public ActionResult EliminaSurveySession(int id)
        {
            try
            {
                surveyService.EliminaSurveySession(id);

                ViewBag.Result = true;
            }
            catch
            {
                ViewBag.Result = false;
            }

            return View();
        }

        public ActionResult VisualizzaSurvey(int id)
        {
            SurveyWithAnswers surveyWithAnswers = surveyService.GetSurveyWithAnswers(id);

            if (surveyWithAnswers != null)
            {
                return View(surveyWithAnswers);
            }
            else
            {
                throw new Exception("Survey Session non trovata");
            }


        }
    }
}
