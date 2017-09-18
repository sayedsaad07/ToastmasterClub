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
                    );
            services.AddIdentity<ApplicationIdentityUser, IdentityRole>(options =>
            {
                //options.Cookies.ApplicationCookie.LoginPath = "/Account/SignIn";
                //options.Password.RequireDigit
            })
            .AddEntityFrameworkStores<BookKeeperContext>()
            .AddDefaultTokenProviders();

            //register classes for IOC
            services.AddScoped<IBookKeeperRepository, BookKeeperRepository>();
            services.AddTransient<IMessageService, FileMessageService>();
            services.AddMvc();
            services.AddMvc(option =>
            {
                //option.Filters.Add(new RequireHttpsAttribute());
            });
            //only first time
            services.AddScoped<InitBookKeeper>();
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
                dbContext.Database.Migrate(); //this will generate the db if it does not exist
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
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
