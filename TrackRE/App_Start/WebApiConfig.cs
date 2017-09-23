using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace TrackRE
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API-2 attribute routing enabled
            config.MapHttpAttributeRoutes();

            
            // Web API-1 conventional routing
            config.Routes.MapHttpRoute(
                name: "DefaultApi",                         
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            /*
            config.Routes.MapHttpRoute(
                name: "ApiRoute",                         
                routeTemplate: "api/{controller}/{action}/{proptype}",
                defaults: new { proptype = RouteParameter.Optional }
            );

            
            routes.MapHttpRoute(
                        name: "CreateUser",
                        routeTemplate: "api/User/{cUser}",
                        defaults: new
                        {
                            controller = "User",
                            action = "CreateUser",
                            cUser = RouteParameter.Optional
                        });

            routes.MapHttpRoute(
                name: "AllGames",
                routeTemplate: "api/Game/{playerId}",
                defaults: new
                {
                    controller = "Game",
                    action = "GetAllGames",
                    playerId = RouteParameter.Optional
                }
            );
             * */
        }
    }
}
