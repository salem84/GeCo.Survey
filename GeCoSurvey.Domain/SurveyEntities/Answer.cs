using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeCoSurvey.Domain
{
    public class Answer
    {
        public int Id { get; set; }

        public virtual SurveySession Session { get; set; }
        public int SessionId { get; set; }

        public virtual Question Domanda { get; set; }
        public int DomandaId { get; set; }

        //public int Value { get; set; }
        public virtual SubQuestion RispostaData { get; set; }
        public int RispostaDataId { get; set; }
    }
}
