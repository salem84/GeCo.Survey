using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Profile;
using System.Web.Security;

namespace GeCoSurvey.Service
{
    public class UserService
    {
        
        public void CreaUtente(string username, string email, UserProfile profileProp, bool skipIfExists)
        {

            string password = "pavimental";

            //Registro l'utente
            MembershipCreateStatus createStatus;
            Membership.CreateUser(username, password, email, null, null, true, null, out createStatus);
            
            if (createStatus == MembershipCreateStatus..Success)
            {
                //Creo il profilo
                var profile = ProfileBase.Create(username);
                profile.SetPropertyValue("Nome", profileProp.Nome);
                profile.SetPropertyValue("Cognome", profileProp.Cognome);
                profile.SetPropertyValue("Area", profileProp.Area);
                profile.SetPropertyValue("Matricola", profileProp.Matricola);
                
                

                profile.Save();
            }
            else if(createStatus == MembershipCreateStatus.DuplicateUserName)
            {
                if(!skipIfExists)
                    throw new Exception("errore creazione utente: username esistente");
            }
            else
            {
                throw new Exception("errore creazione utente");
            }



        }

    }

    
}
