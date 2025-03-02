using System.Web.Http;
using WebActivatorEx;
using TechnomediaTestTask;
using Swashbuckle.Application;
using System.Linq;

namespace TechnomediaTestTask
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var config = GlobalConfiguration.Configuration;

            config.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "TechnomediaTestTask API");
                c.IncludeXmlComments(GetXmlCommentsPath());
                c.DescribeAllEnumsAsStrings();
                c.ApiKey("Bearer")
                    .Description("Please enter JWT with Bearer into field")
                    .Name("Authorization")
                    .In("header");
            })
            .EnableSwaggerUi(c =>
            {
                c.EnableApiKeySupport("Authorization", "header");
            });
        }

        private static string GetXmlCommentsPath()
        {
            return System.String.Format(@"{0}\bin\TechnomediaTestTask.XML",
                System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
