using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookKeeperSPAAngular.Model
{
    public class ApplicationIdentityUser : IdentityUser
    {
        public ApplicationIdentityUser()
        {
        }

        public DateTime FirstBook { get; set; }
    }
}
