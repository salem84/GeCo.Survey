using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Profile;

namespace GeCoSurvey.Service
{
    public class UserProfile : ProfileBase
    {
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Matricola { get; set; }
        public string Area { get; set; }
    }
}
