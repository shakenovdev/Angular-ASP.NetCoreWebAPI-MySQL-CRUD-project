using WebApi.Models;
using WebApi.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebApi.Services.Interfaces
{
    public interface IProfileService
    {
         Task<ProcessResult<ProfileViewModel>> GetProfileAsync(string userName);
         Task<ProcessResult<List<IdeaViewModel>>> GetListAsync(ProfileFilterViewModel model);
         Task<ProcessResult<List<PopularUserViewModel>>> GetPopularUsersAsync();
         Task<ProcessResult<UserSettingsViewModel>> GetUserSettingsAsync();
         Task<ProcessResult> UpdateUserSettingsAsync(UserSettingsViewModel model);
        
    }
}