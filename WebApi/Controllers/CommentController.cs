using DataAccessLayer.Models;
using WebApi.ViewModels;
using WebApi.Services.Interfaces;
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
    public class CommentController : BaseController
    {
        private readonly ICommentService _commentService;
        private readonly ILogger<CommentController> _logger;

        public CommentController(UserManager<ApplicationUser> userManager, 
                                ICommentService commentService,
                                ILogger<CommentController> logger) 
            : base(userManager)
        {
            this._commentService = commentService;
            this._logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate([FromBody] CommentNewViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _commentService.CreateOrUpdateAsync(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Like([FromBody] LikeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _commentService.LikeAsync(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<CommentViewModel>), 200)]
        public async Task<IActionResult> GetList(long ideaId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _commentService.GetListAsync(ideaId);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveOrRestore(long id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _commentService.RemoveOrRestoreAsync(id);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok();
        }
        
    }
}