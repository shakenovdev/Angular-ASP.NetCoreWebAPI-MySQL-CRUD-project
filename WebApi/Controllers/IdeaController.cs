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
    [Authorize]
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<string>), 400)]
    public class IdeaController : BaseController
    {
        private readonly IIdeaService _ideaService;
        private readonly ILogger<IdeaController> _logger;
        
        public IdeaController(UserManager<ApplicationUser> userManager, 
                              IIdeaService ideaService,
                              ILogger<IdeaController> logger) 
            : base(userManager)
        {
            this._ideaService = ideaService;
            this._logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<IdeaViewModel>), 200)]
        public async Task<IActionResult> GetList(FilterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _ideaService.GetListAsync(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result.Value);
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<IdeaViewModel>), 200)]
        public async Task<IActionResult> SearchList(string searchValue, long? lastIdeaId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _ideaService.SearchListAsync(searchValue, lastIdeaId);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate([FromBody] IdeaNewViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            var result = await _ideaService.CreateOrUpdateAsync(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok();
        }
        
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IdeaViewModel), 200)]
        public async Task<IActionResult> GetDetail(long ideaId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _ideaService.GetDetailAsync(ideaId);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Like([FromBody] LikeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _ideaService.LikeAsync(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddToFavorites(long ideaId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _ideaService.AddToFavoritesAsync(ideaId);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveOrRestore(long ideaId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _ideaService.RemoveOrRestoreAsync(ideaId);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok();
        }
    }
}