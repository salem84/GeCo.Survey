using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Profile;
using System.Web.Security;
using System.Data.SqlClient;
using System.Configuration;
using GeCoSurvey.Data;

namespace GeCoSurvey.Service
{
    public interface IUserService
    {
        List<string> GetDipendentiByResponsabile(string responsabileUsername);
        void CreaUtente(string username, Dictionary<string, string> profileProp, bool skipIfExists);
        UserProfile GetUtente(string username);
        void SalvaProfilo(string username, IUserProperties profiloNew);
    }


    public class UserService : IUserService
    {
        //Prendo tutti i dipendenti sotto un responsabile
        public List<string> GetDipendentiByResponsabile(string responsabileUsername)
        {
            //MembershipUser responsabile = Membership.GetUser(responsabileUsername);
            UserProfile responsabile = GetUtente(responsabileUsername);

            string area = responsabile.Area;

            var dipendentiUsernames = GetUsersInAreaAndRole(area, Tipologiche.Ruoli.DIPENDENTI);

            return dipendentiUsernames;
        }


        public List<string> GetUsersInAreaAndRole(string area, string role)
        {
            List<string> usernames = new List<string>();

            string connectionString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            string storedProc = "GetUsersInArea";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(storedProc, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        usernames.Add(reader[0].ToString());
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }
            }


            return usernames;
        }

        public void CreaUtente(string username, Dictionary<string,string> profileProp, bool skipIfExists)
        {
            string email = profileProp["Email"];
            string password = "pavimental";

            //Registro l'utente
            MembershipCreateStatus createStatus;

            username += DateTime.Now.Ticks;
            Membership.CreateUser(username, password, email, null, null, true, null, out createStatus);
            

            if (createStatus == MembershipCreateStatus.Success)
            {
                //Aggiungo il ruolo
                Roles.AddUserToRole(username, "PrimoAccesso");

                //Creo il profilo
                var profile =  ProfileBase.Create(username) as UserProfile;
                profile.Nome = profileProp["Nome"];
                profile.Cognome = profileProp["Cognome"];
                profile.Area = profileProp["Area"];
                profile.Matricola = profileProp["Matricola"];
                /*profile.SetPropertyValue("Nome", profileProp.Nome);
                profile.SetPropertyValue("FirstName", profileProp.Nome);
                profile.SetPropertyValue("Cognome", profileProp.Cognome);
                profile.SetPropertyValue("Area", profileProp.Area);
                profile.SetPropertyValue("Matricola", profileProp.Matricola);*/
                
                

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


        public UserProfile GetUtente(string username)
        {
            var userProfile = ProfileBase.Create(username) as UserProfile;
            return userProfile;
        }


        //Utilizzato per salvare il profilo, nel pannello amministrazione
        public void SalvaProfilo(string username, IUserProperties profiloNew)
        {
            //TODO forse si può fare utilizzando direttamente ProfiloNew ?
            UserProfile profiloOld = GetUtente(username);

            profiloOld.Nome = profiloNew.Nome;
            profiloOld.Cognome = profiloNew.Cognome;
            profiloOld.Matricola = profiloNew.Matricola;
            profiloOld.Area = profiloNew.Area;

            profiloOld.Save();
        }

    }

    
}
