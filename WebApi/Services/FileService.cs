using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using WebApi.Models;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class FileService : BaseService, IFileService
    {
        private readonly IHostingEnvironment _environment;
        public FileService(UserManager<ApplicationUser> userManager,
                           IHttpContextAccessor contextAccessor, 
                           ApplicationDbContext context,
                           IHostingEnvironment environment) 
            : base(userManager, contextAccessor, context)
        {
            this._environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public async Task<ProcessResult<string>> UploadAsync(IFormFile file)
        {
            Func<Task<string>> action = async () => {
                if (!this.CheckImageContentType(file.ContentType))
                    throw new InvalidDataException("The uploaded file has to be .jpg, .jpeg, .png or .png");

                const string uploadFolder = "uploads";
                // Should I save original FileName? Actually, it's not necessary at the moment ¯\_(ツ)_/¯
                string fileName = this.GetUniqueFileName(file.FileName);
                var uploads = Path.Combine(_environment.WebRootPath, uploadFolder);
                var absoluteFilePath = Path.Combine(uploads, fileName);
                string relativeFilePath = Path.Combine(uploadFolder, fileName);
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(absoluteFilePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                return relativeFilePath;
            };

            return await Process.RunAsync(action);
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return  Path.GetFileNameWithoutExtension(fileName)
                    + "_" 
                    + Guid.NewGuid().ToString().Substring(0, 4) 
                    + Path.GetExtension(fileName);
        }

        private bool CheckImageContentType(string fileContentType)
        {
            string[] availableContentTypes = {"image/gif", "image/jpeg", "image/png"};
            if (availableContentTypes.Contains(fileContentType))
                return true;

            return false;
        }
    }
}