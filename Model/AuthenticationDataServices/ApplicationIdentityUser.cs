using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookKeeperSPAAngular.Model
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
        }

        public DateTime FirstBook { get; set; }
    }
}
