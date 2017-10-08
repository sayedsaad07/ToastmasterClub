using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookKeeperSPAAngular.Model.IdentitySecurity
{
    public class TokenProviderOptions
    {
        public string Secret { get; set; }
        public string Iss { get; set; }
        public string Aud { get; set; }
        /*
        public string Path { get; set; } = "/token";
        
        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(5);

        public SigningCredentials SigningCredentials { get; set; }
        */
    }
}
