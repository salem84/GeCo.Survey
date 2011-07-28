using System;
using System.Collections.Generic;
using System.Linq;

namespace GeCoSurvey.Domain
{
    public class SurveyWithAnswers 
    {
        public string NomeRisorsa { get; set; }
        public SurveySession SurveySession { get; set; }
        public List<QuestionWithAnswer> DomandeConRisposte { get; set; }
    }


    public class QuestionWithAnswer : Question
    {
        //public SubQuestion RispostaData { get; set; }
        public int RispostaDataId { get; set; }
        public Question Question { get; set; }

        /*public QuestionWithAnswer()
        {
            //TODO Non si può fare in altro modo sta porcata???

            this.Children = q.Children;
            this.Competenza = q.Competenza;
            this.Id = q.Id;
            this.Survey = q.Survey;
            this.SurveyId = q.SurveyId;
            this.Testo = q.Testo;
        }*/
    }
}