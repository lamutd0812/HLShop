using Microsoft.Owin.Security.OAuth;
using System.Web.Http;

namespace HLShop.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            // phan login: Microsoft.Owin.Security.OAuth;
            // lọc giữa cơ chế login = token (Admin) và login = cookie (client)
            // phai cai them microsoft.AspNet.WebApi.Owin (nuget package)
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}