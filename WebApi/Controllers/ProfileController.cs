using DataAccessLayer.Models;
using WebApi.Services.Interfaces;
using WebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<string>), 400)]
    public class ProfileController : BaseController
    {
        private readonly IProfileService _profileService;
        private readonly ILogger<ProfileController> _logger;
        
        public ProfileController(UserManager<ApplicationUser> userManager, 
                              IProfileService profileService,
                              ILogger<ProfileController> logger) 
            : base(userManager)
        {
            this._profileService = profileService;
            this._logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ProfileViewModel), 200)]
        public async Task<IActionResult> GetInfo(string userName)
        {
            var result = await _profileService.GetProfileAsync(userName);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(result.Value);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<IdeaViewModel>), 200)]
        public async Task<IActionResult> GetIdeaList(ProfileFilterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _profileService.GetListAsync(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(result.Value);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<PopularUserViewModel>), 200)]
        public async Task<IActionResult> GetPopularUsers()
        {
            var result = await _profileService.GetPopularUsersAsync();
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(result.Value);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(UserSettingsViewModel), 200)]
        public async Task<IActionResult> GetUserSettings()
        {
            var result = await _profileService.GetUserSettingsAsync();
            if (!result.Succeeded)
                return BadRequest(result.Errors);
                
            return Ok(result.Value);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateUserSettings([FromBody] UserSettingsViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _profileService.UpdateUserSettingsAsync(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok();
        }
        
    }
}