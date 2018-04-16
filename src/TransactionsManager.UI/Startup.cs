using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TransactionsManager.Domain;
using TransactionsManager.Repository.EF;
using TransactionsManager.Services;
using TransactionsManager.Services.Interfaces;

namespace TransactionsManager.UI
{
    public class Startup
    {
        public static string[] Args { get; set; } = new string[] { };
        private ILogger logger;
        private ILoggerFactory loggerFactory;
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .AddCommandLine(Startup.Args);

            Configuration = builder.Build();
            this.loggerFactory = loggerFactory;
            this.loggerFactory.AddConsole(LogLevel.Information);
            this.loggerFactory.AddDebug();
            this.logger = this.loggerFactory.CreateLogger("Startup");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            logger.LogTrace(Configuration.GetConnectionString("DefaultConnection"));
            services.AddDbContext<TransactionManagerContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<ITransactionRepository, TransactionsEFRepository>();
            services.AddTransient<ITransactionsService, TransactionsService>();

            services.AddMvc();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();

            var requireHttpsMetadata = Configuration.GetSection("Authorization:RequireHttpsMetadata").Value;
            bool parsedRequireHttps;
            bool.TryParse(requireHttpsMetadata, out parsedRequireHttps);

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration.GetSection("Authorization:AuthorityUrl").Value ?? "http://localhost:5000";
                    options.RequireHttpsMetadata = parsedRequireHttps;
                    options.ApiName = Configuration.GetSection("Authorization:ApiName").Value;
                });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";

                    options.Authority = Configuration.GetSection("Authorization:AuthorityUrl").Value ?? "http://localhost:5000";
                    options.RequireHttpsMetadata = parsedRequireHttps;

                    options.ClientId = Configuration.GetSection("Authorization:ClientId").Value;
                    options.SaveTokens = true;
                    options.ClientSecret = Configuration.GetSection("Authorization:ClientSecret").Value;
                    options.ResponseType = "code id_token"; // hybrid flow
                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.Scope.Add("role");
                    options.ClaimActions.Add(new JsonKeyClaimAction("role", "role", "role"));

                    options.Scope.Add("transactions-api");
                    options.Scope.Add("offline_access");
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Assistant", policy => policy.RequireAssertion(p => p.User.Claims
                    .Any(c => c.Type == "role" && (c.Value == "assistant" || c.Value == "admin"))));
                options.AddPolicy("Manager", policy => policy.RequireAssertion(p => p.User.Claims
                    .Any(c => c.Type == "role" && (c.Value == "manager" || c.Value == "admin"))));
                options.AddPolicy("Administrator", policy => policy.RequireAssertion(p => p.User.Claims
                    .Any(c => c.Type == "role" && (c.Value == "admin"))));
            });

            services.AddSingleton<IConfigureOptions<CookieAuthenticationOptions>, ConfigureCookieOptions>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    internal class ConfigureCookieOptions : IConfigureNamedOptions<CookieAuthenticationOptions>
    {
        public ConfigureCookieOptions() { }
        public void Configure(CookieAuthenticationOptions options) { }
        public void Configure(string name, CookieAuthenticationOptions options)
        {
            options.AccessDeniedPath = "/";
        }
    }
}
