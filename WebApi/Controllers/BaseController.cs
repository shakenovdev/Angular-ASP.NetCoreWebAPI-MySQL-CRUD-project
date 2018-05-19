using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace WebApi.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationUser _appUser;
        public ApplicationUser CurrentUser => GetCurrentUser();

        public BaseController(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }

        private ApplicationUser GetCurrentUser()
        {
            if (!User.Identity.IsAuthenticated)
                return null;

            if (_appUser != null)
                return _appUser;

            var userId = User?.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            _appUser = _userManager.FindByIdAsync(userId).Result;
            return _appUser;
        }
    }
}