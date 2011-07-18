using System.Web.Mvc;

namespace GeCoSurvey.Web.Areas.Admin
{
    public class ProvaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Prova";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Prova_default",
                "Prova/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
