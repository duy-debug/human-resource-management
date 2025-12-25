using System.Web.Mvc;

namespace human_resource_management.Areas.HumanResource
{
    public class HumanResourceAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "HumanResource";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "HumanResource_default",
                "HumanResource/{controller}/{action}/{id}",
<<<<<<< HEAD
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "human_resource_management.Areas.HumanResource.Controllers" }
=======
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
>>>>>>> ddff4f5123cbfdd5d8dc84f559dfb2190309d627
            );
        }
    }
}