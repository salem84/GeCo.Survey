using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeCoSurvey.Web.Areas.Admin.Models.Users
{
    public class ProfileViewModel
    {
        public string Username { get; set; }

        public string Matricola { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Area { get; set; }
    }
}