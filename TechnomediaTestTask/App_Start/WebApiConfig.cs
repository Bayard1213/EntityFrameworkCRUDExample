using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using TechnomediaTestTask.Services;

namespace TechnomediaTestTask
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<AuthService>().AsSelf();
            builder.RegisterType<TechnomediaTestTaskDBEntities>().AsSelf();
            builder.RegisterType<AssignmentsService>().AsSelf();
            builder.RegisterType<ClientsService>().AsSelf();
            builder.RegisterType<ReportsService>().AsSelf();
            builder.RegisterType<RequestsService>().AsSelf();
            builder.RegisterType<TeamsService>().AsSelf();
            builder.RegisterType<WorkLogsService>().AsSelf();

            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            
            //
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
