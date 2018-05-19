using WebApi.Models;
using WebApi.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebApi.Services.Interfaces
{
    public interface ICommentService
    {
         Task<ProcessResult> CreateOrUpdateAsync(CommentNewViewModel model);
         Task<ProcessResult> LikeAsync(LikeViewModel model);
         Task<ProcessResult<List<CommentViewModel>>> GetListAsync(long ideaId);
         Task<ProcessResult> RemoveOrRestoreAsync(long id);
    }
}