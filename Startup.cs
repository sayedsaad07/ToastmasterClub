using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BookKeeperSPAAngular.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BookKeeperSPAAngular
{
    public class Startup
    {
        private IConfiguration _Configuration;
        IHostingEnvironment _env;
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _env = env;
            _Configuration = configuration;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            _Configuration = builder.Build();

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_Configuration);
            //BookKeeperContext
            services.AddDbContext<BookKeeperContext>(options
                => options.UseSqlServer(
                        _Configuration.GetConnectionString("DefaultConnection")
                    //,
                    //optionbuilder => optionbuilder.MigrationsAssembly("BookKeeperSPAAngularMigrationfile")
                    )
                    //U can use imnMemory DB for simplicity
                    //need to add Microsoft.EntityFrameworkCore.InMemory
                    //options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
                    );
            services.AddIdentity<ApplicationIdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<BookKeeperContext>()
            .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                //options.Password.RequiredLength = 8;
                //options.Password.RequireNonAlphanumeric = false;
                //options.Password.RequireUppercase = true;
                //options.Password.RequireLowercase = false;
                //options.Password.RequiredUniqueChars = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            ConfigureApplicationCookie(services);
            //register classes for IOC
            services.AddScoped<IBookKeeperRepository, BookKeeperRepository>();
            services.AddTransient<IMessageService, FileMessageService>();
            //add cross origin resource sharing
            services.AddCors(cfg =>
            {
                cfg.AddPolicy("sayedsaad07", builder =>
                {
                    builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("http://sayedsaad07.com");
                });
                cfg.AddPolicy("AnyGET", builder =>
                {
                    builder.AllowAnyHeader()
                    .WithMethods("GET")
                    .AllowAnyOrigin();
                });
            });
            services.AddMvc(option =>
            {
                if (_env.IsDevelopment())
                {
                    option.SslPort = 44398;
                }
                option.Filters.Add(new RequireHttpsAttribute());
            });
            //only first time
            services.AddScoped<InitBookKeeper>();
        }

        private static void ConfigureApplicationCookie(IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = TimeSpan.FromDays(150);
                options.LoginPath = "/Account/Login"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login
                options.LogoutPath = "/Account/Logout"; // If the LogoutPath is not set here, ASP.NET Core will default to /Account/Logout
                options.AccessDeniedPath = "/Account/AccessDenied"; // If the AccessDeniedPath is not set here, ASP.NET Core will default to /Account/AccessDenied
                options.SlidingExpiration = true;
                options.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = (context) =>
                    {
                        if (context.Request.Path.StartsWithSegments("/api")
                        && context.Response.StatusCode == 200)
                        {
                            context.Response.StatusCode = 401;
                        }
                        return Task.CompletedTask;
                    },
                    OnRedirectToAccessDenied = (context) =>
                    {
                        if (context.Request.Path.StartsWithSegments("/api")
                        && context.Response.StatusCode == 200)
                        {
                            context.Response.StatusCode = 403;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app
            , ILoggerFactory loggerFactory
            , BookKeeperContext dbContext
            , InitBookKeeper seeder)
        {

            loggerFactory.AddConsole(_Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            if (_env.IsDevelopment())
            {//dotnet run --environment "Development"

                app.UseDeveloperExceptionPage();
                //dbContext.Database.Migrate(); //this will generate the db if it does not exist
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            //use identity before using MVC
            app.UseAuthentication();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
            if (_env.IsDevelopment())
            { seeder.SeedData().Wait(); }
        }
    }
}
