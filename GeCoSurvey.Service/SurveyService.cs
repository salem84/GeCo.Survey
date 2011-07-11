using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeCoSurvey.Domain;
using GeCoSurvey.Data.Infrastructure;
using GeCoSurvey.Data;

namespace GeCoSurvey.Service
{
    public interface ISurveyService
    {
        IEnumerable<Survey> GetSurveys(bool active);
        
        Survey GetSurvey(int idSurvey);
        SurveyWithAnswers GetSurveyWithAnswers(int idSurveySession);
        
        //List<Answer> GetRisposte(int idSurvey, string username);
        
        void SalvaSurvey(string username, List<Answer> risposte);
        void SalvaSurveyRevisionato(int idSurveySession, List<Answer> risposte);

        
    }

    public class SurveyService : ISurveyService
    {
        private readonly IRepository<Survey> reposSurvey;
        //private readonly IRepository<Answer> reposAnswer;
        private readonly IRepository<SurveySession> reposSurveySession;
        private readonly IRepository<SubQuestion> reposSubQuestion;
        private readonly DipendentiService dipendentiService;

        private readonly UnitOfWork unityOfWork;

        public SurveyService(IRepository<Survey> reposSurvey,
            IRepository<SurveySession> reposSurveySession,
            IRepository<SubQuestion> reposSubQuestion,
            DipendentiService dipendentiService,
            UnitOfWork unityOfWork)
        {
            this.reposSurvey = reposSurvey;
            //this.reposAnswer = reposAnswer;
            this.reposSurveySession = reposSurveySession;
            this.reposSubQuestion = reposSubQuestion;
            this.unityOfWork = unityOfWork;

            this.dipendentiService = dipendentiService;
        }

        
        public IEnumerable<Survey> GetSurveys(bool active)
        {
            var surveys = reposSurvey.GetMany(s => s.Active == active);
            return surveys;
        }

        public Survey GetSurvey(int id)
        {
            var survey = reposSurvey.GetById(id);
            return survey;
        }

        public void SalvaSurvey(string username, List<Answer> risposte)
        {
            //TODO verifico che non esista già una sessione a nome di quell'utente?
            
            //Creo la sessione
            SurveySession session = new SurveySession
            {
                User = username,
                SurveyId = 1,
                Risposte = new List<Answer>()
            };

            //Salvo le risposte
            foreach (var r in risposte)
            {
                r.Session = session;  
                session.Risposte.Add(r);
            }

            reposSurveySession.Add(session);

            unityOfWork.Commit();
        }


        /*public List<Answer> GetRisposte(int idSurvey, string username)
        {
            var result = reposAnswer.GetMany(a => a.Session.User == username && a.Session.SurveyId == idSurvey);
            //var result = reposAnswer.GetMany(a => a.Session.User == username && a.Domanda.SurveyId == surveyId).ToList();
            return result.ToList();
        }*/

        public SurveySession GetSurveySession(int idSurveySession)
        {
            var result = reposSurveySession.Get(ss => ss.Id == idSurveySession);
            return result;
        }

        

        public void SalvaSurveyRevisionato(int idSurveySession, List<Answer> risposte)
        {
            Dipendente dipendente = new Dipendente
            {
                Nome = "pippo",
                Cognome = "boh",
                Matricola = "112",
                DataNascita = DateTime.Parse("25/01/1984"),
            };

            //List<LivelloConoscenza> livelli = dipendentiService.GetLivelliConoscenza();
            var session = GetSurveySession(idSurveySession);
            var domande = session.Survey.Questions;

            foreach (var r in risposte)
            {
                
                var domanda = domande.Single(d => d.Id == r.DomandaId);
                var subquestion = reposSubQuestion.Get(sb => sb.Id == r.RispostaDataId);                

                ConoscenzaCompetenza cc = new ConoscenzaCompetenza
                {
                    CompetenzaId = domanda.CompetenzaId,
                    LivelloConoscenzaId = subquestion.LivelloConoscenzaId
                };

                dipendente.Conoscenze.Add(cc);
            }

            dipendentiService.SalvaDipendente(dipendente);
        }

        public SurveyWithAnswers GetSurveyWithAnswers(int idSurveySession)
        {
            SurveySession surveySession = GetSurveySession(idSurveySession);

            Survey survey = surveySession.Survey;
            List<Answer> risposte = surveySession.Risposte.ToList();

            var qWr = from q in survey.Questions
                      let risposta = risposte.Single(r => r.DomandaId == q.Id)
                      select new QuestionWithAnswer
                      {
                          RispostaDataId = risposta.RispostaDataId,
                          Question = q
                      };

            SurveyWithAnswers surveyWithAnswers = new SurveyWithAnswers
            {
                SurveySession = surveySession,
                DomandeConRisposte = qWr.ToList()
            };

            return surveyWithAnswers;
        }
    }
}
