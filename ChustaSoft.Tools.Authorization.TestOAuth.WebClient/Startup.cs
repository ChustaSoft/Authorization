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
                    opt.DefaultScheme = "Cookies";
                    opt.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", opt => 
                {
                    opt.Authority = "https://localhost:44319/";
                    opt.ClientId = "client-test-id";
                    opt.ClientSecret = "secret";
                    opt.ResponseType = "code id_token";
                    //opt.UsePkce = true;
                    //opt.ResponseMode = "query";
                    opt.SignInScheme = "Cookies";
                    opt.Scope.Add("test-api");
                    opt.SaveTokens = true;
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
