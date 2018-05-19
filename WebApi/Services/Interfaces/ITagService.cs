using WebApi.Models;
using WebApi.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Services.Interfaces
{
    public interface ITagService
    {
         Task<ProcessResult<List<TagViewModel>>> GetPopularAsync(int count);
    }
}