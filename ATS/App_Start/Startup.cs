using ATS.Infrastructure;
using ATS.Providers;
using ATS.Service.Google;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;

[assembly: OwinStartup(typeof(ATS.App_Start.Startup))]
namespace ATS.App_Start
{
    public class Startup
    {
        //Social Providers for Web Api Pipeline
        public static OAuthBearerAuthenticationOptions _OAuthBearerOptions { get; private set; }
        public static GoogleOAuth2AuthenticationOptions _googleAuthOptions { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration httpConfig = new HttpConfiguration();

            ConfigureOAuthTokenGeneration(app);
            ConfigureOAuthTokenConsumption(app);
            ConfigureOAuthExternal(app);
            ConfigureWebApi(httpConfig);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.UseWebApi(httpConfig);

        }
        private void ConfigureOAuthExternal(IAppBuilder app)
        {
            //user a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
            _OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            app.UseCookieAuthentication(new CookieAuthenticationOptions{
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager, "ExternalAccessToken"))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            //Google will be our 3rd party
            _googleAuthOptions = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "403744615341-hej6i7qeakd0trpnc02r57rub34qa75k.apps.googleusercontent.com",
                ClientSecret = "rV9TV4DjrRahLPOlijTQSmQX",
                Provider = new GoogleAuthProvider()
            };
            app.UseGoogleAuthentication(_googleAuthOptions);

        }
        private void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {
            // Configure to use a single instance respectively per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                //For Dev enviroment only (on production should be AllowInsecureHttp = false)
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/oauth/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                //validate the Resource owner user credential in a custom class named “CustomOAuthProvider”.
                Provider = new CustomOAuthProvider(),
                AccessTokenFormat = new CustomJwtFormat("http://localhost:57020")
            };

            // OAuth 2.0 Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            //for third party(google)
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        private void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {

            var issuer = "http://localhost:57020";
            string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            byte[] audienceSecret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["as:AudienceSecret"]);

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                //API will now be restricted to JWT Tokens
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audienceId },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)
                    }
                });
        }

        private void ConfigureWebApi(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}