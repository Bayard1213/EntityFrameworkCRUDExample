using System.Configuration;
using Owin;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security;

namespace TechnomediaTestTask
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var issuer = ConfigurationManager.AppSettings["Jwt:Issuer"];
            var key = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["Jwt:Key"]);
            var securityKey = new SymmetricSecurityKey(key);

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    IssuerSigningKey = securityKey
                }
            });
        }
    }
}