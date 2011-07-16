using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Profile;
using System.Web;
using System.Web.Security;

namespace GeCoSurvey.Service
{
    public class UserProfile : ProfileBase
    {
        public static UserProfile GetProfile()
        {
            return Create(HttpContext.Current.Request.IsAuthenticated ?
                   HttpContext.Current.User.Identity.Name : HttpContext.Current.Request.AnonymousID,
                   HttpContext.Current.Request.IsAuthenticated) as UserProfile;
        }

        public static UserProfile GetUserProfile(string username)
        {
            return Create(username) as UserProfile;
        }

        public static UserProfile GetUserProfile()
        {
            return Create(Membership.GetUser().UserName) as UserProfile;
        }

        // Add the Profile properties starting here
        // using the same property name and SQL type
        // as exists in your new profile table


        [CustomProviderData("Nome;nvarchar")]
        public virtual string Nome
        {
            get
            {
                return ((string)(this.GetPropertyValue("Nome")));
            }
            set
            {
                this.SetPropertyValue("Nome", value);
            }
        }

        [CustomProviderData("Cognome;nvarchar")]
        public virtual string Cognome
        {
            get
            {
                return ((string)(this.GetPropertyValue("Cognome")));
            }
            set
            {
                this.SetPropertyValue("Cognome", value);
            }
        }

        [CustomProviderData("Area;nvarchar")]
        public virtual string Area
        {
            get
            {
                return ((string)(this.GetPropertyValue("Area")));
            }
            set
            {
                this.SetPropertyValue("Area", value);
            }
        }

        [CustomProviderData("Matricola;nvarchar")]
        public virtual string Matricola
        {
            get
            {
                return ((string)(this.GetPropertyValue("Matricola")));
            }
            set
            {
                this.SetPropertyValue("Matricola", value);
            }
        }

    }

}
