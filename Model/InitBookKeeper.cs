using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookKeeperSPAAngular.Model
{
    public class InitBookKeeper
    {
        private BookKeeperContext _DBContext;
        private UserManager<ApplicationIdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleMgr;

        public InitBookKeeper(BookKeeperContext dbcontext
            , UserManager<ApplicationIdentityUser> userManager
            , RoleManager<IdentityRole> rolemgr)
        {
            _DBContext = dbcontext;
            _userManager = userManager;
            _roleMgr = rolemgr;
        }

        public async Task SeedData()
        {

            try
            {
                var user = await _userManager.FindByEmailAsync("sayedsaad07@gmail.com");
                if (user == null)
                {
                    if (!(await _roleMgr.RoleExistsAsync("Admin")))
                    {
                        var role = new IdentityRole();
                        role.Name = "Admin";
                        await _roleMgr.CreateAsync(role);

                        var model = new { Email = "sayedsaad07@gmail.com", Password = "Test_123" };
                        user = new ApplicationIdentityUser { UserName = model.Email, Email = model.Email, SecurityStamp = "111" };
                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            var roleResult = await _userManager.AddToRoleAsync(user, "Admin");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            if (!_DBContext.BookKeeper.Any())
            {
                _DBContext.BookKeeper.AddRange(
                    new BookKeeper() { BookName = "Entrepreneur book", Category = "Motivation", BookUrl = "Www.blogpost.com", Rating = 5, Userid = 1, Startreading = DateTime.Now.AddDays(-30), StopReading = DateTime.Now },
                    new BookKeeper() { BookName = "Toastmaster book", Category = "Motivation", BookUrl = "Www.blogpost.com", Rating = 5, Userid = 1, Startreading = DateTime.Now.AddDays(-30), StopReading = DateTime.Now },
                    new BookKeeper() { BookName = "4 Hour Week book", Category = "Motivation", BookUrl = "Www.blogpost.com", Rating = 5, Userid = 1, Startreading = DateTime.Now.AddDays(-30), StopReading = DateTime.Now },
                    new BookKeeper() { BookName = "Think fast and slow book", Category = "Motivation", BookUrl = "Www.blogpost.com", Rating = 5, Userid = 1, Startreading = DateTime.Now.AddDays(-30), StopReading = DateTime.Now },
                    new BookKeeper() { BookName = "Think fast and slow book", Category = "Motivation", BookUrl = "Www.blogpost.com", Rating = 5, Userid = 2, Startreading = DateTime.Now.AddDays(-30), StopReading = DateTime.Now }
                    );
                await _DBContext.SaveChangesAsync();

            }
        }
    }
}
