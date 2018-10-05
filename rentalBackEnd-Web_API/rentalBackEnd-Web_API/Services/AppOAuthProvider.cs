using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.OAuth;
using rentalBackEnd_Web_API.Models;

namespace rentalBackEnd_Web_API.Services
{
    public class AppOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            await Task.Run(()=> context.Validated());
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var manager = new UserManager<ApplicationUser>(userStore);
            var user = await manager.FindAsync(context.UserName,context.Password);

            if (user != null)
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                List<Claim> claims = new List<Claim>
                {
                 new Claim("UserName", user.UserName),
                 new Claim("E-Mail", user.Email),
                 new Claim("Full Name", user.fullName),
                 new Claim("Role",manager.GetRoles(user.Id)[0]),
                 new Claim("LoggedIn", DateTime.Now.ToString()),
                 new Claim(ClaimTypes.NameIdentifier, user.Id)
                };

                identity.AddClaims(claims);
                context.Validated(identity);
            }
            else
            {
                return;
            }
        }




    }
}