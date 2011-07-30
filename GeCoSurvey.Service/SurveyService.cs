using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeCoSurvey.Domain;
using GeCoSurvey.Data.Infrastructure;
using GeCoSurvey.Data;
using GeCo.Infrastructure;

namespace GeCoSurvey.Service
{
    public interface ISurveyService
    {
        IEnumerable<Survey> GetSurveys(bool active);
        IEnumerable<SurveyWithState> GetSurveysWithState(string username, bool active);

        Survey GetSurvey(int idSurvey);
        SurveyWithAnswers GetSurveyWithAnswers(int idSurveySession);
        
        //List<Answer> GetRisposte(int idSurvey, string username);

        void SalvaSurvey(int surveyId, string username, List<Answer> risposte);
        void SalvaSurveyRevisionato(int idSurveySession, List<Answer> risposte);

        IEnumerable<SurveySession> GetSurveySessionsByResponsabile(string responsabile);
    }

    public class SurveyService : ISurveyService
    {
        private readonly IRepository<Survey> reposSurvey;
        //private readonly IRepository<Answer> reposAnswer;
        private readonly IRepository<SurveySession> reposSurveySession;
        private readonly IRepository<SubQuestion> reposSubQuestion;
        //private readonly IRepository<ResponsabiliDipendenti> reposResponsabiliDipendenti;
        private readonly IDipendentiService dipendentiService;
        private readonly IUserService userService;

        private readonly UnitOfWork unityOfWork;

        public SurveyService(IRepository<Survey> reposSurvey,
            IRepository<SurveySession> reposSurveySession,
            IRepository<SubQuestion> reposSubQuestion,
            //IRepository<ResponsabiliDipendenti> reposResponsabiliDipendenti,
            IDipendentiService dipendentiService,
            IUserService userService,
            UnitOfWork unityOfWork)
        {
            this.reposSurvey = reposSurvey;
            //this.reposAnswer = reposAnswer;
            this.reposSurveySession = reposSurveySession;
            this.reposSubQuestion = reposSubQuestion;
            //this.reposResponsabiliDipendenti = reposResponsabiliDipendenti;
            this.userService = userService;
            this.unityOfWork = unityOfWork;


            this.dipendentiService = dipendentiService;
        }

        
        public IEnumerable<Survey> GetSurveys(bool active)
        {
            var surveys = reposSurvey.GetMany(s => s.Active == active);
            return surveys;
        }

        public IEnumerable<SurveyWithState> GetSurveysWithState(string username, bool active)
        {
            //Parto dalle sessioni per vedere quelli compilati
            var surveysCompilati = reposSurveySession.GetMany(s => s.User == username).Select(s => s.Survey).Where(s => s.Active == active);

            //Tutti quanti
            var surveys = GetSurveys(active);

            //Incrocio
            var surveysConStato = from s in surveys
                                 let compilato = surveysCompilati.Contains(s, su => su.Id)
                                 select new SurveyWithState
                                 {
                                     Survey = s,
                                     Compilato = compilato
                                 };

            return surveysConStato;
        }


        public Survey GetSurvey(int id)
        {
            var survey = reposSurvey.GetById(id);
            return survey;
        }

        public void SalvaSurvey(int surveyId, string username, List<Answer> risposte)
        {
            //TODO verifico che non esista già una sessione a nome di quell'utente?
            
            //Creo la sessione
            SurveySession session = new SurveySession
            {
                User = username,
                SurveyId = surveyId,
                Risposte = new List<Answer>()
            };

            //Salvo le risposte
            foreach (var r in risposte)
            {
                r.Session = session;  
                session.Risposte.Add(r);
            }

            //La segno come non revisionata
            session.Revisionato = false;

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
            Dipendente dipendente = new Dipendente();            


            //List<LivelloConoscenza> livelli = dipendentiService.GetLivelliConoscenza();
            var session = GetSurveySession(idSurveySession);
            
            //Leggo le informazioni dell'utente
            UserProfile profilo = userService.GetUtente(session.User);
            dipendente.Nome = profilo.Nome;
            dipendente.Cognome = profilo.Cognome;
            dipendente.Matricola = profilo.Matricola;
            
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

            //devo segnare la surveysession come revisionata
            session.Revisionato = true;
            reposSurveySession.Update(session);
            unityOfWork.Commit();
        }

        public SurveyWithAnswers GetSurveyWithAnswers(int idSurveySession)
        {
            SurveySession surveySession = GetSurveySession(idSurveySession);

            if (surveySession != null)
            {

                Survey survey = surveySession.Survey;
                List<Answer> risposte = surveySession.Risposte.ToList();

                //Prendo tutte le domande e risposte
                var qWr = from q in survey.Questions
                          let risposta = risposte.Single(r => r.DomandaId == q.Id)
                          select new QuestionWithAnswer
                          {
                              RispostaDataId = risposta.RispostaDataId,
                              Question = q
                          };

                //Carico le informazioni sul profilo
                string username = surveySession.User;
                UserProfile profilo = userService.GetUtente(username);

                //Creo l'oggetto per la view
                SurveyWithAnswers surveyWithAnswers = new SurveyWithAnswers
                {
                    SurveySession = surveySession,
                    DomandeConRisposte = qWr.ToList(),
                    NomeRisorsa = profilo.ToString()
                };

                return surveyWithAnswers;
            }
            else
            {
                return null;
            }
        }


        public IEnumerable<SurveySession> GetSurveySessionsByResponsabile(string responsabile)
        {
            //Prendo i dipendenti associati al responsabile
            IEnumerable<string> dipendenti = userService.GetDipendentiByResponsabile(responsabile);

            //Di questi utenti, alcuni hanno iniziato e/o completato il questionario, quindi avrò gli oggetti SurveySessions
            IEnumerable<SurveySession> sessionsCompletate = reposSurveySession.GetMany(ss => dipendenti.Contains(ss.User));
            IEnumerable<string> utentiConQuestionario = sessionsCompletate.Where(s => s.Completato == true).Select(s => s.User);

            //Altri non hanno ancora fatto nulla
            IEnumerable<string> utentiSenzaQuestionario = dipendenti.Except(utentiConQuestionario);

            //Creo quindi delle sessions fittizie per la View, 
            //scartando l'utente responsabile che non ha sicuramente compilato il questionario 
            var sessionsNonCompletate = from d in utentiSenzaQuestionario
                                        where d != responsabile
                                        select new SurveySession
                                        {
                                            User = d
                                        };
            //Faccio l'unione di tutto
            var sessions = sessionsCompletate.Union(sessionsNonCompletate);

            return sessions;
        }
    }
}
