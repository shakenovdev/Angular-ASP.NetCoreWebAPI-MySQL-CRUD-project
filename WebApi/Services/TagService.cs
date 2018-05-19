using DataAccessLayer;
using DataAccessLayer.Models;
using WebApi.Models;
using WebApi.Services.Interfaces;
using WebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public class TagService : BaseService, ITagService
    {
        public TagService(UserManager<ApplicationUser> userManager,
                          IHttpContextAccessor contextAccessor, 
                          ApplicationDbContext context) 
            : base(userManager, contextAccessor, context)
        {
        }

        public async Task<ProcessResult<List<TagViewModel>>> GetPopularAsync(int count)
        {
            Func<Task<List<TagViewModel>>> action = async () =>
            {
                var result = await context.relIdeaTags.Include(x => x.Tag)
                                                .Where(x => !x.IsDeleted)
                                                .GroupBy(x => new { x.TagId, x.Tag.Name })
                                                .OrderByDescending(x => x.Count())
                                                .Take(count)
                                                .Select(x => new TagViewModel
                                                {
                                                    Id = x.Key.TagId,
                                                    Name = x.Key.Name
                                                }).ToListAsync();
                                                
                return result;
            };

            return await Process.RunAsync(action);
        }
    }
}