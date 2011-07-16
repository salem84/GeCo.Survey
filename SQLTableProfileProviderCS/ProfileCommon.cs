using System;
using System.Web.Profile;
using System.Web.Security;
using System.Web.Configuration;
using System.Web;

namespace Microsoft.Samples
{
    public class ProfileCommon : System.Web.Profile.ProfileBase
    {

        public static ProfileCommon GetProfile()
        {
            return Create(HttpContext.Current.Request.IsAuthenticated ?
                   HttpContext.Current.User.Identity.Name : HttpContext.Current.Request.AnonymousID,
                   HttpContext.Current.Request.IsAuthenticated) as ProfileCommon;
        }

        public static ProfileCommon GetUserProfile(string username)
        {
            return Create(username) as ProfileCommon;
        }

        public static ProfileCommon GetUserProfile()
        {
            return Create(Membership.GetUser().UserName) as ProfileCommon;
        }

        // Add the Profile properties starting here
        // using the same property name and SQL type
        // as exists in your new profile table


[CustomProviderData("FirstName;nvarchar")]
public virtual string FirstName
{
    get
    {
        return ((string)(this.GetPropertyValue("FirstName")));
    }
    set
    {
        this.SetPropertyValue("FirstName", value);
    }
}

        [CustomProviderData("LastName;nvarchar")]
        public virtual string LastName
        {
            get
            {
                return ((string)(this.GetPropertyValue("LastName")));
            }
            set
            {
                this.SetPropertyValue("LastName", value);
            }
        }


        [CustomProviderData("Age;int")]
        public virtual int Age
        {
            get
            {
                return ((int)(this.GetPropertyValue("Age")));
            }
            set
            {
                this.SetPropertyValue("Age", value);
            }
        }

        [CustomProviderData("DateOfBirth;datetime")]
        public virtual DateTime DateOfBirth
        {
            get
            {
                return ((DateTime)(this.GetPropertyValue("DateOfBirth")));
            }
            set
            {
                this.SetPropertyValue("DateOfBirth", value);
            }
        }
    }

}