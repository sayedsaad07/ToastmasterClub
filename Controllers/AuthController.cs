using BookKeeperSPAAngular.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookKeeperSPAAngular.Controllers
{
    public class AuthController : Controller
    {
        private BookKeeperContext _DbContext;
        private SignInManager<ApplicationIdentityUser> _SignInManager;
        private ILogger _logger;

        public AuthController(BookKeeperContext dbContext,
            SignInManager<ApplicationIdentityUser> signInManager
            , ILogger<AuthController> logger)
        {
            _DbContext = dbContext;
            _SignInManager = signInManager;
            _logger = logger;
        }
         
        [HttpPost("api/auth/login")] 
        public async Task<IActionResult> LogIn([FromBody] UserCredentialModel model)
        {
            try
            {
                var result = await _SignInManager.PasswordSignInAsync(model.UserName , model.Password , false , false);
                if (result.Succeeded)
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Invalid user name. throw exception{ex}");
            }
            return BadRequest("Invalid login info");
        }
    }
}
