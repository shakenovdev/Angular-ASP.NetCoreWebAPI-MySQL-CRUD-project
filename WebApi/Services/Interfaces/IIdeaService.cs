using WebApi.Models;
using WebApi.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebApi.Services.Interfaces
{
    public interface IIdeaService
    {
        Task<ProcessResult> CreateOrUpdateAsync(IdeaNewViewModel model);
        Task<ProcessResult<List<IdeaViewModel>>> GetListAsync(FilterViewModel model);
        Task<ProcessResult<List<IdeaViewModel>>> SearchListAsync(string searchValue, long? lastIdeaId);
        Task<ProcessResult<IdeaViewModel>> GetDetailAsync(long ideaId);
        Task<ProcessResult> LikeAsync(LikeViewModel model);
        Task<ProcessResult> AddToFavoritesAsync(long ideaId);
        Task<ProcessResult> RemoveOrRestoreAsync(long ideaId);
    }
}