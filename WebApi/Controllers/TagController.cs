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
    public class TagController : BaseController
    {
        private readonly ITagService _tagService;
        private readonly ILogger<TagController> _logger;
        
        public TagController(UserManager<ApplicationUser> userManager, 
                             ITagService tagService,
                             ILogger<TagController> logger) 
            : base (userManager)
        {
            _tagService = tagService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<TagViewModel>), 200)]
        public async Task<IActionResult> GetPopular(int count = 10)
        {
            var result = await _tagService.GetPopularAsync(count);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(result.Value);
        }
        
    }
}