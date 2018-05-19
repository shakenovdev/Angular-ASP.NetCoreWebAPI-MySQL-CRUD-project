using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public abstract class BaseService
    {
        public readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        protected readonly ApplicationDbContext context;
        private HttpContext _httpContext { get { return _contextAccessor.HttpContext; } }
        private ApplicationUser _appUser;
        public ApplicationUser CurrentUser => GetCurrentUser();

        public BaseService(UserManager<ApplicationUser> userManager,
                           IHttpContextAccessor contextAccessor,
                           ApplicationDbContext context)
        {
            this.userManager = userManager;
            this._contextAccessor = contextAccessor;
            this.context = context;
        }

        private ApplicationUser GetCurrentUser()
        {
            if (!_httpContext.User.Identity.IsAuthenticated)
                return new ApplicationUser();

            if (_appUser != null)
                return _appUser;

            var userId = _httpContext.User?.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            _appUser = userManager.FindByIdAsync(userId).Result;
            return _appUser;
        }

        
        protected (bool isCreated, TResult result) GetOrCreateEntity<TResult>(
            DbSet<TResult> sourceCollection,
            Expression<Func<TResult, bool>> whereConditions = null)
            where TResult : class, new()
        {
            var isCreated = false;
            TResult result = null;
            
            if (whereConditions != null)
                result = sourceCollection.FirstOrDefault(whereConditions);

            if (result == null)
            {
                isCreated = true;
                result = new TResult();
                sourceCollection.Add(result);
            }

            var creatorId = typeof(TResult).GetProperty("CreatorId");
            if (creatorId != null && isCreated)
                creatorId.SetValue(result, CurrentUser.Id);

            var modifierId = typeof(TResult).GetProperty("ModifierId");
            if (modifierId != null)
                modifierId.SetValue(result, CurrentUser.Id);

            var createDate = typeof(TResult).GetProperty("CreateDate");
            if (createDate != null && isCreated)
                createDate.SetValue(result, DateTime.UtcNow);

            var modifyDate = typeof(TResult).GetProperty("ModifyDate");
            if (modifyDate != null)
                modifyDate.SetValue(result, DateTime.UtcNow);

            var hversion = typeof(TResult).GetProperty("HVersion");
            if (hversion != null)
                hversion.SetValue(result, (int)hversion.GetValue(result) + 1);

            return (isCreated, result);
        }

        protected async Task<(bool isCreated, TResult result)> GetOrCreateEntityAsync<TResult>(
            DbSet<TResult> sourceCollection,
            Expression<Func<TResult, bool>> whereConditions = null)
            where TResult : class, new()
        {
            var isCreated = false;
            TResult result = null;

            if (whereConditions != null)
                result = await sourceCollection.FirstOrDefaultAsync(whereConditions);

            if (result == null)
            {
                isCreated = true;
                result = new TResult();
                await sourceCollection.AddAsync(result);
            }

            var creatorId = typeof(TResult).GetProperty("CreatorId");
            if (creatorId != null && isCreated)
                creatorId.SetValue(result, CurrentUser.Id);

            var modifierId = typeof(TResult).GetProperty("ModifierId");
            if (modifierId != null)
                modifierId.SetValue(result, CurrentUser.Id);

            var createDate = typeof(TResult).GetProperty("CreateDate");
            if (createDate != null && isCreated)
                createDate.SetValue(result, DateTime.UtcNow);

            var modifyDate = typeof(TResult).GetProperty("ModifyDate");
            if (modifyDate != null)
                modifyDate.SetValue(result, DateTime.UtcNow);

            var hversion = typeof(TResult).GetProperty("HVersion");
            if (hversion != null)
                hversion.SetValue(result, (int)hversion.GetValue(result) + 1);

            return (isCreated, result);
        }
    }
}