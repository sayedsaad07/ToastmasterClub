using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookKeeperSPAAngular.Model
{
    public class BookKeeperContext : IdentityDbContext<ApplicationIdentityUser>//: DbContext
    {
        public BookKeeperContext(DbContextOptions<BookKeeperContext> contextOption) : base(contextOption) { }
        public DbSet<BookKeeper> BookKeeper { get; set; }
        public DbSet<BookKeeperUser> BookKeeperUser { get; set; }
    }

    public class BookKeeper
    {
        public int BookKeeperId { get; set; }
        public int Userid { get; set; }
        
        public string BookName { get; set; }

        public string Category { get; set; }

        public DateTime Startreading { get; set; }
        public DateTime StopReading { get; set; }
        public short Rating { get; set; }
        public string BookUrl { get; set; }

        public BookKeeperUser BookKeeperUser { get; set; }
    }

    public class BookKeeperUser
    {
        public int BookKeeperUserId {get; set;}
        public string UserName { get; set; }
        public string Email { get; set; }

    }
}
