using HLShop.Data;
using HLShop.Model.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(HLShop.Web.App_Start.Startup))]

namespace HLShop.Web.App_Start
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(HLShopDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            app.CreatePerOwinContext<UserManager<ApplicationUser>>(CreateManager);
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/oauth/token"),
                Provider = new AuthorizationServerProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                AllowInsecureHttp = true
            });
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "1724156397871880",
            //   appSecret: "398039cc7588d52f87a7adcefecc3210");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "712161982861-4d9bdgfvf6pti1vviifjogopqdqlft56.apps.googleusercontent.com",
            //    ClientSecret = "T0cgiSG6Gi7BKMr-fCCkdErO"
            //});
        }

        public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
        {
            public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
            {
                await Task.Run(() =>
                        context.Validated()
                    );
            }

            public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
            {
                var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

                if (allowedOrigin == null) allowedOrigin = "*";

                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

                UserManager<ApplicationUser> userManager = context.OwinContext.GetUserManager<UserManager<ApplicationUser>>();
                ApplicationUser user;
                try
                {
                    user = await userManager.FindAsync(context.UserName, context.Password);
                }
                catch
                {
                    // Could not retrieve the user due to error.
                    context.SetError("server_error");
                    context.Rejected();
                    return;
                }
                if (user != null)
                {
                    ClaimsIdentity identity = await userManager.CreateIdentityAsync(user,
                        DefaultAuthenticationTypes.ExternalBearer);
                    context.Validated(identity);
                }
                else
                {
                    context.SetError("invalid_grant", "Tài khoản hoặc mật khẩu không đúng.'");
                    context.Rejected();
                }
            }
        }

        private static UserManager<ApplicationUser> CreateManager(IdentityFactoryOptions<UserManager<ApplicationUser>> options, IOwinContext context)
        {
            var userStore = new UserStore<ApplicationUser>(context.Get<HLShopDbContext>());
            var owinManager = new UserManager<ApplicationUser>(userStore);
            return owinManager;
        }
    }
}