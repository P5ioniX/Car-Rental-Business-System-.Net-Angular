using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using rentalBackEnd_Web_API.Services;

[assembly: OwinStartup(typeof(rentalBackEnd_Web_API.Startup))]

namespace rentalBackEnd_Web_API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            

            app.UseCors(CorsOptions.AllowAll);

            

            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/retrievetoken"),
                Provider = new AppOAuthProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(60),
                AllowInsecureHttp = true
            };
            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
