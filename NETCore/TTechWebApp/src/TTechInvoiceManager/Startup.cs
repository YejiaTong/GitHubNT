using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;

using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

using NTWebApp.DBAccess;

namespace NTWebApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            Configuration = builder.Build();
            var AppSettingsSection = Configuration.GetSection("AppSettings");
            DBManager.SetConnectionString(Configuration.GetConnectionString("MySQLConnection"));
            DBMapperFactory.InitializeDBMapperFactory();
            AutoMapperFactory.InitializeAutoMapperFactory();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            // The order of adding services is important due to program pipeline
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator", policy => policy.RequireRole("Administrator"));
                options.AddPolicy("Visitor", policy => policy.RequireRole("Visitor"));
            });

            services.AddAuthentication(
                options => options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme);

            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            // The main cookie middleware to application - CookieMiddlewareInstance
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationScheme = "CookieMiddlewareInstance",
                LoginPath = new PathString("/Account/Login/"),
                AccessDeniedPath = new PathString("/Account/Forbidden/"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                CookieDomain = ".ttechcode.com",
                ExpireTimeSpan = TimeSpan.FromDays(7),
                SlidingExpiration = true
            });

            // Add the cookie middleware - Facebook
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                LoginPath = new PathString("/loginFacebook"),
                LogoutPath = new PathString("/logoutFacebook"),
                AccessDeniedPath = new PathString("/Account/Forbidden/"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                CookieDomain = ".ttechcode.com",
                ExpireTimeSpan = TimeSpan.FromDays(7),
                SlidingExpiration = true
            });

            // Add the cookie middleware - Google
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                LoginPath = new PathString("/loginGoogle"),
                LogoutPath = new PathString("/logoutGoogle"),
                AccessDeniedPath = new PathString("/Account/Forbidden/"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                CookieDomain = ".ttechcode.com",
                ExpireTimeSpan = TimeSpan.FromDays(7),
                SlidingExpiration = true
            });

            // Add the cookie middleware - LinkedIn
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                LoginPath = new PathString("/loginLinkedIn"),
                LogoutPath = new PathString("/logoutLinkedIn"),
                AccessDeniedPath = new PathString("/Account/Forbidden/"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                CookieDomain = ".ttechcode.com",
                ExpireTimeSpan = TimeSpan.FromDays(7),
                SlidingExpiration = true
            });

            FacebookOAuth2Setup(app);
            GooglePlusOAuth2Setup(app);
            LinkedInOAuth2Setup(app);

            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Redirect}/{action=LoginRouter}");
            });
        }

        public void FacebookOAuth2Setup(IApplicationBuilder app)
        {
            // Add the OAuth2 middleware - Facebook
            app.UseFacebookAuthentication(new FacebookOptions
            {
                AuthenticationScheme = Configuration.GetConnectionString("FacebookAuthenticationScheme"),

                AppId = Configuration.GetConnectionString("FacebookClientId"),
                AppSecret = Configuration.GetConnectionString("FacebookClientSecret"),
                Scope = { "email" },
                Fields = { "name", "email" },
                SaveTokens = true,

                Events = new OAuthEvents
                {
                    // The OnCreatingTicket event is called after the user has been authenticated and the OAuth middleware has
                    // created an auth ticket. We need to manually call the UserInformationEndpoint to retrieve the user's information,
                    // parse the resulting JSON to extract the relevant information, and add the correct claims.
                    OnCreatingTicket = async context =>
                    {
                        // Retrieve user info
                        var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                        request.Headers.Add("x-li-format", "json"); // Tell LinkedIn we want the result in JSON, otherwise it will return XML

                        var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
                        response.EnsureSuccessStatusCode();

                        // Extract the user info object
                        var user = JObject.Parse(await response.Content.ReadAsStringAsync());

                        // Add the Name Identifier claim
                        var userId = user.Value<string>("id");
                        if (!string.IsNullOrEmpty(userId))
                        {
                            context.Identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId, ClaimValueTypes.String, context.Options.ClaimsIssuer));
                        }

                        // Add the Name claim
                        var name = user.Value<string>("name");
                        if (!string.IsNullOrEmpty(name))
                        {
                            context.Identity.AddClaim(new Claim(ClaimTypes.Name, name, ClaimValueTypes.String, context.Options.ClaimsIssuer));
                        }

                        // Add the email address claim
                        var email = user.Value<string>("email");
                        if (!string.IsNullOrEmpty(email))
                        {
                            context.Identity.AddClaim(new Claim(ClaimTypes.Email, email, ClaimValueTypes.String,
                                context.Options.ClaimsIssuer));
                        }

                        var pictureUrl = @"http://graph.facebook.com/" + userId + @"/picture?type=large";

                        if (!string.IsNullOrEmpty(pictureUrl))
                        {
                            context.Identity.AddClaim(new Claim("profile-picture", pictureUrl, ClaimValueTypes.String,
                                context.Options.ClaimsIssuer));
                        }

                        context.Identity.AddClaim(new Claim("externalSource", "Facebook", ClaimValueTypes.String));

                        context.Identity.AddClaim(new Claim(ClaimTypes.Role, "External", ClaimValueTypes.String));
                    }
                }
            });

            // Listen for requests on the /login path, and issue a challenge to log in with the LinkedIn middleware
            app.Map("/loginFacebook", builder =>
            {
                builder.Run(async context =>
                {
                    // Return a challenge to invoke the LinkedIn authentication scheme
                    await context.Authentication.ChallengeAsync(Configuration.GetConnectionString("FacebookAuthenticationScheme"), properties: new AuthenticationProperties() { RedirectUri = new PathString("/Redirect/LoginRed") });
                });
            });

            // Listen for requests on the /logout path, and sign the user out
            app.Map("/logoutFacebook", builder =>
            {
                builder.Run(async context =>
                {
                    // Sign the user out of the authentication middleware (i.e. it will clear the Auth cookie)
                    await context.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    // Redirect the user to the home page after signing out
                    context.Response.Redirect(new PathString("/Redirect/LogoutRed"));
                });
            });
        }

        public void GooglePlusOAuth2Setup(IApplicationBuilder app)
        {
            // Add the OAuth2 middleware - Google
            app.UseGoogleAuthentication(new GoogleOptions
            {
                AuthenticationScheme = Configuration.GetConnectionString("GoogleAuthenticationScheme"),

                ClientId = Configuration.GetConnectionString("GoogleClientId"),
                ClientSecret = Configuration.GetConnectionString("GoogleClientSecret"),

                CallbackPath = new PathString("/signin-google"),

                Events = new OAuthEvents
                {
                    // The OnCreatingTicket event is called after the user has been authenticated and the OAuth middleware has
                    // created an auth ticket. We need to manually call the UserInformationEndpoint to retrieve the user's information,
                    // parse the resulting JSON to extract the relevant information, and add the correct claims.
                    OnCreatingTicket = async context =>
                    {
                        // Retrieve user info
                        var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                        request.Headers.Add("x-li-format", "json"); // Tell LinkedIn we want the result in JSON, otherwise it will return XML

                        var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
                        response.EnsureSuccessStatusCode();

                        // Extract the user info object
                        var user = JObject.Parse(await response.Content.ReadAsStringAsync());

                        // Add the Name Identifier claim
                        var userId = user.Value<string>("id");
                        if (!string.IsNullOrEmpty(userId))
                        {
                            context.Identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId, ClaimValueTypes.String, context.Options.ClaimsIssuer));
                        }

                        // Add the Name claim
                        var name = user.Value<string>("displayName");
                        if (!string.IsNullOrEmpty(name))
                        {
                            context.Identity.AddClaim(new Claim(ClaimTypes.Name, name, ClaimValueTypes.String, context.Options.ClaimsIssuer));
                        }

                        // Add the email address claim
                        var emailJArray = (JArray)user["emails"];
                        if(emailJArray != null && emailJArray.Count != 0)
                        {
                            var email = emailJArray[0].Value<string>("value");
                            if (!string.IsNullOrEmpty(email))
                            {
                                context.Identity.AddClaim(new Claim(ClaimTypes.Email, email, ClaimValueTypes.String,
                                    context.Options.ClaimsIssuer));
                            }
                        }

                        // Add the Profile Picture claim
                        var image = user["image"];
                        if (image != null)
                        {
                            var pictureUrl = image.Value<string>("url");

                            if (!string.IsNullOrEmpty(pictureUrl))
                            {
                                context.Identity.AddClaim(new Claim("profile-picture", pictureUrl, ClaimValueTypes.String,
                                    context.Options.ClaimsIssuer));
                            }
                        }

                        context.Identity.AddClaim(new Claim("externalSource", "Google", ClaimValueTypes.String));

                        context.Identity.AddClaim(new Claim(ClaimTypes.Role, "External", ClaimValueTypes.String));
                    }
                }
            });

            // Listen for requests on the /login path, and issue a challenge to log in with the LinkedIn middleware
            app.Map("/loginGoogle", builder =>
            {
                builder.Run(async context =>
                {
                    // Return a challenge to invoke the LinkedIn authentication scheme
                    await context.Authentication.ChallengeAsync(Configuration.GetConnectionString("GoogleAuthenticationScheme"), properties: new AuthenticationProperties() { RedirectUri = new PathString("/Redirect/LoginRed") });
                });
            });

            // Listen for requests on the /logout path, and sign the user out
            app.Map("/logoutGoogle", builder =>
            {
                builder.Run(async context =>
                {
                    // Sign the user out of the authentication middleware (i.e. it will clear the Auth cookie)
                    await context.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    // Redirect the user to the home page after signing out
                    context.Response.Redirect(new PathString("/Redirect/LogoutRed"));
                });
            });
        }

        public void LinkedInOAuth2Setup(IApplicationBuilder app)
        {
            // Add the OAuth2 middleware - LinkedIn
            app.UseOAuthAuthentication(new OAuthOptions
            {
                AuthenticationScheme = Configuration.GetConnectionString("LinkedInAuthenticationScheme"),

                ClientId = Configuration.GetConnectionString("LinkedInClientId"),
                ClientSecret = Configuration.GetConnectionString("LinkedInClientSecret"),

                CallbackPath = new PathString("/auth/linkedin/callback"),

                AuthorizationEndpoint = "https://www.linkedin.com/oauth/v2/authorization",
                TokenEndpoint = "https://www.linkedin.com/oauth/v2/accessToken",
                UserInformationEndpoint = "https://api.linkedin.com/v1/people/~:(id,formatted-name,email-address,picture-url)",

                Scope = { "r_basicprofile", "r_emailaddress" },

                Events = new OAuthEvents
                {
                    // The OnCreatingTicket event is called after the user has been authenticated and the OAuth middleware has
                    // created an auth ticket. We need to manually call the UserInformationEndpoint to retrieve the user's information,
                    // parse the resulting JSON to extract the relevant information, and add the correct claims.
                    OnCreatingTicket = async context =>
                    {
                        // Retrieve user info
                        var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                        request.Headers.Add("x-li-format", "json"); // Tell LinkedIn we want the result in JSON, otherwise it will return XML

                        var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
                        response.EnsureSuccessStatusCode();

                        // Extract the user info object
                        var user = JObject.Parse(await response.Content.ReadAsStringAsync());

                        // Add the Name Identifier claim
                        var userId = user.Value<string>("id");
                        if (!string.IsNullOrEmpty(userId))
                        {
                            context.Identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId, ClaimValueTypes.String, context.Options.ClaimsIssuer));
                        }

                        // Add the Name claim
                        var formattedName = user.Value<string>("formattedName");
                        if (!string.IsNullOrEmpty(formattedName))
                        {
                            context.Identity.AddClaim(new Claim(ClaimTypes.Name, formattedName, ClaimValueTypes.String, context.Options.ClaimsIssuer));
                        }

                        // Add the email address claim
                        var email = user.Value<string>("emailAddress");
                        if (!string.IsNullOrEmpty(email))
                        {
                            context.Identity.AddClaim(new Claim(ClaimTypes.Email, email, ClaimValueTypes.String,
                                context.Options.ClaimsIssuer));
                        }

                        // Add the Profile Picture claim
                        var pictureUrl = user.Value<string>("pictureUrl");
                        if (!string.IsNullOrEmpty(pictureUrl))
                        {
                            context.Identity.AddClaim(new Claim("profile-picture", pictureUrl, ClaimValueTypes.String,
                                context.Options.ClaimsIssuer));
                        }

                        context.Identity.AddClaim(new Claim("externalSource", "LinkedIn", ClaimValueTypes.String));

                        context.Identity.AddClaim(new Claim(ClaimTypes.Role, "External", ClaimValueTypes.String));
                    }
                }
            });

            // Listen for requests on the /login path, and issue a challenge to log in with the LinkedIn middleware
            app.Map("/loginLinkedIn", builder =>
            {
                builder.Run(async context =>
                {
                    // Return a challenge to invoke the LinkedIn authentication scheme
                    await context.Authentication.ChallengeAsync(Configuration.GetConnectionString("LinkedInAuthenticationScheme"), properties: new AuthenticationProperties() { RedirectUri = new PathString("/Redirect/LoginRed") });
                });
            });

            // Listen for requests on the /logout path, and sign the user out
            app.Map("/logoutLinkedIn", builder =>
            {
                builder.Run(async context =>
                {
                    // Sign the user out of the authentication middleware (i.e. it will clear the Auth cookie)
                    await context.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    // Redirect the user to the home page after signing out
                    context.Response.Redirect(new PathString("/Redirect/LogoutRed"));
                });
            });
        }
    }
}
