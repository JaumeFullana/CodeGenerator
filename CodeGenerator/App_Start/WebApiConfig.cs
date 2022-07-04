using System.Web.Http;

namespace CodeGenerator
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{userMail}",
                defaults: new { userMail = RouteParameter.Optional }
            );
        }
    }
}
