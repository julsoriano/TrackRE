using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using System;
using TrackRE.Models;

// OAuth Providers
//using Owin.Security.Providers.Yahoo;
using Owin.Security.Providers.LinkedIn;

namespace TrackRE
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            // Consider for incorporation: http://www.codeproject.com/Articles/247336/Twitter-OAuth-authentication-using-Net
            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication();

            app.UseFacebookAuthentication(                              // https://trackre.azurewebsites.net/, 
                appId: "240280922803698",
                appSecret: "a4a466a69e8219dc99a35be416bcf34f");

            app.UseGoogleAuthentication(
                clientId: "888586712341-no4p09sp9l7tqpnil1nu1jlqhnboi3jm.apps.googleusercontent.com",
                clientSecret: "OFxJ3Gr_9VvJlEOogNU_52fw");

            app.UseLinkedInAuthentication(
               clientId: "75va23crtwij6z",
                clientSecret: "w9gxpRq8pVTlvPYa");
        }
    }
}