using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeCoSurvey.Service;
using GeCoSurvey.Domain;
using GeCoSurvey.Web.Models;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace GeCoSurvey.Web.Controllers
{
    public class SurveysController : Controller
    {
        private readonly ISurveyService surveyService;
        private readonly IDipendentiService dipendentiService;
        private readonly LogWriter logger;

        public SurveysController(ISurveyService surveyService,
            IDipendentiService dipendentiService,
            LogWriter logger
            )
        {
            this.surveyService = surveyService;
            this.dipendentiService = dipendentiService;
            this.logger = logger;
        }

        /// <summary>
        /// Visualizza i questionari da compilare
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Dipendenti, Administrators")]
        public ActionResult Index()
        {
            ViewBag.Message = "Ciao";

            string username = User.Identity.Name;

            logger.Write(string.Format("L'utente {0} ", username));

            //Segno per ogni survey, quali sono stati già compilati dall'utente
            var surveys = surveyService.GetSurveysWithState(username, true);

            return View(surveys);
        }

        /// <summary>
        /// Visualizza tutti i questionari che è possibile revisionare per l'utente correntemente loggato
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Responsabili, Administrators")]
        public ActionResult Visualizza()
        {
            string responsabile = User.Identity.Name;

            var result = surveyService.GetSurveySessionsByResponsabile(responsabile);

            return View(result);
        }

        /// <summary>
        /// Utilizzato dai dipendenti per compilare il questionario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Dipendenti, Administrators")]
        public ActionResult Compila(int id)
        {
            var survey = surveyService.GetSurvey(id);
            return View(survey);
        }


        [HttpPost]
        public ActionResult Compila(int id, FormCollection formCollection)
        {
            //Prendo tutti i valori dei radiobutton (filtro per quelli che iniziano con 'c')
            var form = formCollection.AllKeys.Where(k=> k.StartsWith("c")).ToDictionary(k => k, v => formCollection[v]);

            List<Answer> risposte = new List<Answer>();
            //salva risposte
            foreach (var coppia in form)
            {
                Answer answer = new Answer();
                string strQuestion = coppia.Key.Substring(1); //salto la 'c' (messa sul cshtml come prefisso per l'id)
                answer.DomandaId = Convert.ToInt32(strQuestion);
                
                string strSubQuestion = coppia.Value;
                answer.RispostaDataId = Convert.ToInt32(strSubQuestion);
                
                risposte.Add(answer);
            }

            surveyService.SalvaSurvey(id, User.Identity.Name, risposte);

            logger.Write(string.Format("L'utente {0} ha compilato il questionario", User.Identity.Name));
            return RedirectToAction("Success");
        }

        /// <summary>
        /// Utilizzato dai responsabili per revisionare un questionaro compilato
        /// </summary>
        /// <param name="id">SurveySessionId</param>
        /// <returns></returns>
        [Authorize(Roles = "Responsabili, Administrators")]
        public ActionResult Revisiona(int id)
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

            logger.Write(string.Format("L'utente {0} ha revisionato il questionario", User.Identity.Name));

            return RedirectToAction("Success");
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}
