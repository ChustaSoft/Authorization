using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;

namespace ChustaSoft.Tools.Authorization.TestOAuth.WebClient
{
    public class Startup
    {

        public readonly IConfiguration _configuration;


        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services
                .AddAuthentication(opt =>
                {
                    opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, opt => 
                {
                    opt.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    opt.Authority = "https://localhost:44319/";
                    opt.ClientId = "client-test-web_ui";
                    opt.ClientSecret = "secret";
                    opt.ResponseType = "code";
                    opt.UsePkce = true;
                    //opt.ResponseMode = "query";
                    opt.Scope.Add("roles");
                    opt.Scope.Add("client-test-web_api");
                    opt.SaveTokens = true;
                    opt.GetClaimsFromUserInfoEndpoint = true;
                });



            IdentityModelEventSource.ShowPII = true;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

    }
}
