using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeCoSurvey.Domain
{
    public class SurveySession
    {
        public int Id { get; set; }

        //Da mod per utilizzare classe User
        public string User { get; set; }

        //public virtual Survey Survey { get; set; }
        //public int SurveyId { get; set; }

    }
}
