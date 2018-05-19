using DataAccessLayer.Models;
using WebApi.ViewModels;
using WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<string>), 400)]
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<ApplicationUser> userManager,
                                ITokenService tokenService,
                                IEmailService emailService)
            : base (userManager)
        {
            this._userManager = userManager;
            this._tokenService = tokenService;
            this._emailService = emailService;
        }


        // TODO: NLog thinks that |Authorization was successful for user: (null)|
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> SignIn([FromBody] SignInViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("Wrong email or password");

            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (!isEmailConfirmed)
                return BadRequest("The email is not confirmed");

            var result = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!result)
                return BadRequest("Wrong email or password");
            
            return Ok(_tokenService.Generate(user));
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> SignUp([FromBody] SignUpViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newUser = new ApplicationUser 
            {
                UserName = model.Name,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
                return BadRequest("Sorry, an unexpected error occured");

            #region Send confirmation email
            var confirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var callbackUrl = Url.Action("ConfirmEmail",
                                        "Account", 
                                        new { userId = newUser.Id, code = confirmationCode },
                                        HttpContext.Request.Scheme);
            var message = $"Follow the link to complete registration: <a href='{callbackUrl}'>link</a>";
            await _emailService.SendEmailAsync(model.Email, "Confirm your email", message, message);
            #endregion

            return Ok("We've sent you an email to verify that the address is correct");
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
                return BadRequest("User or confirmation code is empty");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User is not found");

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok("Email is confirmed");
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult ExtendToken()
        {
            return Ok(_tokenService.Generate(CurrentUser));
        }

    }
}