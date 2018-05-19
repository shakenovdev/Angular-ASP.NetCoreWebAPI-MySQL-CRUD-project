using DataAccessLayer.Models;
using WebApi.Services.Interfaces;
using WebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<string>), 400)]
    public class FileController : BaseController
    {
        private readonly IFileService _fileService;
        private readonly ILogger<FileController> _logger;

        public FileController(UserManager<ApplicationUser> userManager, 
                              IFileService fileService,
                              ILogger<FileController> logger) 
            : base(userManager)
        {
            this._fileService = fileService;
            this._logger = logger;
        }

        [HttpPost]
        [RequestSizeLimit(500_000)]  // less than 500 KB
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _fileService.UploadAsync(file);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var link = new { link = result.Value };
            return Ok(link);
        }
    }
}