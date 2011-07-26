using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeCoSurvey.Domain
{
    public class SurveyWithState
    {
        public Survey Survey { get; set; }
        public bool Compilato { get; set; }
    }
}
