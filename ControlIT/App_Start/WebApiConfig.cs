using Autofac;
using Autofac.Integration.WebApi;
using ControlIT.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace ControlIT
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            var connString = ConfigurationManager.ConnectionStrings["ad"]?.ConnectionString;

            builder.RegisterType<Repositorio>()
                .WithParameter("connString", connString);

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
           
        }
    }
}
