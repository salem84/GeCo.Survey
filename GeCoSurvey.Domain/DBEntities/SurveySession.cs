using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeCoSurvey.Domain
{
    public class SurveySession
    {
        public int Id { get; set; }

        public string User { get; set; }

        public virtual Survey Survey { get; set; }
        public int SurveyId { get; set; }

        public virtual ICollection<Answer> Risposte { get; set; }

        public bool Completato
        {
            get { return Risposte.Count() == 0 ? false : true; }
        }

        public SurveySession()
        {
            Risposte = new List<Answer>();
        }


    }
}
