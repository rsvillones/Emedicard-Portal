using System.Web.Mvc;

namespace WebUI.Areas.CorporateAdministrator
{
    public class CorporateAdministratorAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CorporateAdministrator";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CorporateAdministrator_default",
                "CorporateAdministrator/{controller}/{action}/{accountCode}",
                new { controller = "AccountManager", action = "Index", accountCode = UrlParameter.Optional }
            );
        }
    }
}