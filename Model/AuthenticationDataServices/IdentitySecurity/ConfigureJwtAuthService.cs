using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookKeeperSPAAngular.Model.IdentitySecurity
{
    public class ConfigureAuthenticationService
    {
        private IConfigurationRoot _Configuration;
        public ConfigureAuthenticationService(
            IConfigurationRoot configuration)
        {
            _Configuration = configuration;
        }
        
        public void ConfigureJwtBearerOptionse(JwtBearerOptions options)
        {
            var audienceConfig = _Configuration.GetSection("Audience");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!  
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim  
                ValidateIssuer = true,
                ValidIssuer = audienceConfig["Iss"],

                // Validate the JWT Audience (aud) claim  
                ValidateAudience = true,
                ValidAudience = audienceConfig["Aud"],

                // Validate the token expiry  
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = tokenValidationParameters;
        }


        public void ConfigureCookieOptions(CookieAuthenticationOptions options)
        {
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
        }
        
    }
}
