using WebApi.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebApi.Services.Interfaces
{
    public interface IFileService
    {
         Task<ProcessResult<string>> UploadAsync(IFormFile file);
    }
}