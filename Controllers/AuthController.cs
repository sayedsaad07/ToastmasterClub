using BookKeeperSPAAngular.Model;
using BookKeeperSPAAngular.Model.IdentitySecurity;
using BookKeeperSPAAngular.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookKeeperSPAAngular.Controllers
{
    public class AuthController : Controller
    {
        private BookKeeperContext _DbContext;
        private SignInManager<ApplicationUser> _SignInManager;
        private UserManager<ApplicationUser> _userManager;
        private IPasswordHasher<ApplicationUser> _PasswordHasher;
        private ILogger _logger;
        private IOptions<TokenProviderOptions> _appSettings;

        public AuthController(BookKeeperContext dbContext,
            SignInManager<ApplicationUser> signInManager
            , UserManager<ApplicationUser> userManager
            , IPasswordHasher<ApplicationUser> hasher
            , ILogger<AuthController> logger
            , IOptions<TokenProviderOptions> settings)
        {
            _DbContext = dbContext;
            _SignInManager = signInManager;
            _userManager = userManager;
            //used for validating user password
            _PasswordHasher = hasher;
            _logger = logger;
            _appSettings = settings;
        }

        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        [HttpPost("api/auth/Register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        // For more information on how to enable account confirmation and password reset please 
                        //visit http://go.microsoft.com/fwlink/?LinkID=532713
                        // Send an email with this link
                        //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                        //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                        //    "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                        await _SignInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation(3, "User created a new account with password.");
                        //return RedirectToAction(nameof(HomeController.Index), "Home");
                        return Ok("Account created successfully");
                    }
                    return BadRequest($"Invalid registration info: {result}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Invalid user name. throw exception{ex}");
            }
            return BadRequest("Invalid registration info");
        }


        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        [HttpPost("api/auth/login")]
        public async Task<IActionResult> LogIn([FromBody] UserCredentialModel model)
        {
            try
            {
                var result = await _SignInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
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

        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        [HttpPost("api/auth/token")]
        public async Task<IActionResult> GetToken([FromBody] UserCredentialModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    if (_PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) == PasswordVerificationResult.Success)
                    {
                        var response = new TokenGenerator(_appSettings).GetJwt(user.Email);
                        _logger.LogInformation($"My token is bearer {response} ");
                        return Ok(response);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Invalid user name. throw exception{ex}");
            }
            return BadRequest("Invalid login info");
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost("api/auth/logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
                await _SignInManager.SignOutAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Invalid user name. throw exception{ex}");
            }
            return BadRequest("Unable to  logout");
        }
    }
}
