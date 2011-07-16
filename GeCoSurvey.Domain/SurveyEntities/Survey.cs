using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeCoSurvey.Domain
{
    public class Survey
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<Question> Questions { get; set; }

        //Proprietà inversa
        public virtual ICollection<SurveySession> SurveySessions { get; set; }

        public Survey()
        {
            Questions = new List<Question>();
        }
    }
}
