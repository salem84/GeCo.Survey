using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeCoSurvey.Service;

namespace GeCoSurvey.Test
{
    [TestClass]
    public class Services
    {
        [TestMethod]
        public void UserTest()
        {
            string username = "test"+DateTime.Now.Ticks.ToString().Substring(14);

            UserService userService = new UserService();
            
            Dictionary<string, string> profile = new Dictionary<string, string>();

            profile.Add("Email", username + "@pavimental.fake");
            profile.Add("Matricola", "110");
            profile.Add("Nome", "pippo");
            profile.Add("Cognome", "poppo");
            profile.Add("Area", "area1");


            userService.CreaUtente(username, profile, true);

            UserProfile p = userService.GetUtente(username);

            Assert.Equals(p.Nome, profile["Nome"]);
        }
    }
}
