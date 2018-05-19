using DataAccessLayer;
using DataAccessLayer.Models;
using DataAccessLayer.Enums;
using WebApi.Models;
using WebApi.Services.Interfaces;
using WebApi.ViewModels;
using WebApi.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace WebApi.Services
{
    public class ProfileService : BaseService, IProfileService
    {
        public ProfileService(UserManager<ApplicationUser> userManager, 
                           IHttpContextAccessor contextAccessor, 
                           ApplicationDbContext context) 
            : base(userManager, contextAccessor, context)
        {
        }

        public async Task<ProcessResult<ProfileViewModel>> GetProfileAsync(string userName)
        {
            // TODO: PictureUrl, RegistrationDate
            Func<Task<ProfileViewModel>> action = async () =>
            {
                var user = await this.userManager.FindByNameAsync(userName);

                var createdIdeas = context.Ideas.Where(x => x.CreatorId == user.Id && (!x.IsDeleted || x.CreatorId == CurrentUser.Id));
                var createdIdeasIds = await createdIdeas.Select(x => x.Id).ToListAsync();
                var favoritedIdeas = context.relIdeaFavorites.Where(x => x.CreatorId == user.Id && (!x.IsDeleted || x.CreatorId == CurrentUser.Id));
                var votedIdeas = context.relIdeaLikes.Where(x => x.CreatorId == user.Id && !x.IsDeleted);
                var votedComments = context.relCommentLikes.Where(x => x.CreatorId == user.Id && (!x.IsDeleted || x.CreatorId == CurrentUser.Id));

                var sharedCount = await createdIdeas.CountAsync();
                var favoritedCount = await favoritedIdeas.CountAsync();
                var likeCount = await votedIdeas.Where(x => x.Vote == Vote.Like).CountAsync()
                                + await votedComments.Where(x => x.Vote == Vote.Like).CountAsync();
                var dislikeCount = await votedIdeas.Where(x => x.Vote == Vote.Dislike).CountAsync()
                                + await votedComments.Where(x => x.Vote == Vote.Dislike).CountAsync();
                var contributedTags = await context.relIdeaTags.Include(x => x.Tag)
                                                               .Where(x => createdIdeasIds.Contains(x.IdeaId))
                                                               .GroupBy(x => new { x.TagId, x.Tag.Name })
                                                               .OrderByDescending(x => x.Count())
                                                               .Take(10)
                                                               .Select(x => new TagViewModel
                                                               {
                                                                   Id = x.Key.TagId,
                                                                   Name = x.Key.Name
                                                               }).ToListAsync();

                var result = await context.Users.Where(x => x.Id == user.Id)
                                                .Select(x => new ProfileViewModel
                                                {
                                                    Name = x.UserName,
                                                    PictureUrl = String.IsNullOrEmpty(x.AvatarURL) ? "person.png" : x.AvatarURL,
                                                    RegistrationDate = DateTime.Now,
                                                    SharedCount = sharedCount,
                                                    FavoritedCount = favoritedCount,
                                                    LikeCount = likeCount,
                                                    DislikeCount = dislikeCount,
                                                    Rating = x.Rating,
                                                    ContributedTags = contributedTags 
                                                }).SingleAsync();
                
                return result;
            };
            
            return await Process.RunAsync(action);
        }
        
        public async Task<ProcessResult<List<IdeaViewModel>>> GetListAsync(ProfileFilterViewModel model)
        {
            Func<Task<List<IdeaViewModel>>> action = async () =>
            {
                var user = await this.userManager.FindByNameAsync(model.UserName);

                IQueryable<Idea> result = context.Ideas.Where(x => x.CreatorId == user.Id && (!x.IsDeleted || x.CreatorId == CurrentUser.Id));
                var takeSize = model.TakeSize ?? 10;
                var currentUserId = CurrentUser?.Id ?? 0;

                switch (model.Kind)
                {
                    case Kind.Shared:
                        result = result.OrderByDescending(x => x.Id);
                        if (model.LastIdeaId != null)
                            result = result.Where(x => x.Id < model.LastIdeaId);
                        break;
                    case Kind.Favorited:
                        IQueryable<relIdeaFavorite> favorited = context.relIdeaFavorites.Where(x => x.CreatorId == user.Id && !x.IsDeleted)
                                                                                        .OrderByDescending(x => x.CreateDate);

                        if (model.LastIdeaId != null)
                        {
                            var lastFavoritedCreateDate = await favorited.Where(x => x.IdeaId == model.LastIdeaId)
                                                                         .Select(x => x.CreateDate)
                                                                         .SingleAsync();
                            favorited = favorited.Where(x => x.CreateDate < lastFavoritedCreateDate);
                        }
                        
                        result = favorited.Select(x => x.Idea);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("filter value is not valid");
                }

                var ideaList = await result.Include(x => x.Tags)
                                           .Include(x => x.Likes)
                                           .Include(x => x.Comments)
                                           .Include(x => x.Favorites)
                                           .Take(takeSize)
                                           .Select(x => new IdeaViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Article = x.Article,
                    Tags = x.Tags.Where(t => !t.IsDeleted).Select(t => new TagViewModel
                    {
                        Id = t.TagId,
                        Name = t.Tag.Name
                    }).ToList(),
                    Creator = new CreatorViewModel()
                    {
                        Id = x.CreatorId,
                        Name = x.CreatorUser.UserName,
                        AvatarURL = String.IsNullOrEmpty(x.CreatorUser.AvatarURL) ? "person.png" : x.CreatorUser.AvatarURL
                    },
                    CreatedDate = x.CreateDate,
                    CurrentUserLike = x.Likes.Where(l => l.CreatorId == currentUserId)
                                             .Select(l => l.Vote).SingleOrDefault(),
                    CurrentUserIsFavorited = x.Favorites.Where(f => f.CreatorId == CurrentUser.Id)
                                                        .Select(f => f.Value).SingleOrDefault(),
                    LikeCount = x.LikeCount,
                    ViewCount = x.ViewCount,
                    CommentCount = x.Comments.Where(c => !c.IsDeleted || c.CreatorId == CurrentUser.Id).Count(),
                    IsDeleted = x.IsDeleted
                }).ToListAsync();

                return ideaList;
            };
            
            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<List<PopularUserViewModel>>> GetPopularUsersAsync()
        {
            Func<Task<List<PopularUserViewModel>>> action = async () => 
            {
                var users = await this.userManager.Users.Where(x => x.EmailConfirmed)
                                                        .Select(x => new PopularUserViewModel
                                                        {
                                                            UserName = x.UserName,
                                                            AvatarURL = String.IsNullOrEmpty(x.AvatarURL) ? "person.png" : x.AvatarURL,
                                                            Rating = x.Rating
                                                        }).OrderByDescending(x => x.Rating)
                                                        .Take(10)
                                                        .ToListAsync();

                return users;
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<UserSettingsViewModel>> GetUserSettingsAsync()
        {
            Func<Task<UserSettingsViewModel>> action = async () => 
            {
                var user = await this.userManager.FindByIdAsync(CurrentUser.Id.ToString());
                var userSettings =  new UserSettingsViewModel
                                    {
                                        UserName = user.UserName,
                                        AvatarURL = String.IsNullOrEmpty(user.AvatarURL) ? "person.png" : user.AvatarURL
                                    };

                return userSettings;
            };
            
            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> UpdateUserSettingsAsync(UserSettingsViewModel model)
        {
            Func<Task> action = async () =>
            {
                var user = await this.userManager.FindByIdAsync(CurrentUser.Id.ToString());
                
                user.NormalizedUserName = model.UserName.ToUpper();
                user.UserName = model.UserName;
                user.AvatarURL = model.AvatarURL;

                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }
    }
}