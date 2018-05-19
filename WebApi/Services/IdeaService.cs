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
    public class IdeaService : BaseService, IIdeaService
    {
        public IdeaService(UserManager<ApplicationUser> userManager, 
                           IHttpContextAccessor contextAccessor, 
                           ApplicationDbContext context) 
            : base(userManager, contextAccessor, context)
        {
        }

        public async Task<ProcessResult> CreateOrUpdateAsync(IdeaNewViewModel model)
        {
            Func<Task> action = async () => 
            {
                var ideaEntity = await GetOrCreateEntityAsync(context.Ideas, x => x.Id == model.Id);
                var idea = ideaEntity.result;
                if (!ideaEntity.isCreated && CurrentUser.Id != idea.CreatorId)
                    throw new InvalidOperationException("You're not owner of requested idea");

                idea.Title = model.Title;
                idea.Article = model.Article;
                await context.SaveChangesAsync();

                // find or create-new tag and connect it to idea
                for (int i = 0; i < model.Tags.Count; i++)
                {
                    var tagEntity = await GetOrCreateEntityAsync(context.Tags, x => x.Name == model.Tags[i]);
                    var tag = tagEntity.result;
                    if (tagEntity.isCreated)
                        tag.Name = model.Tags[i];
                    await context.SaveChangesAsync();

                    var ideaTag = (await GetOrCreateEntityAsync(context.relIdeaTags)).result;
                    ideaTag.IdeaId = idea.Id;
                    ideaTag.TagId = tag.Id;
                    await context.SaveChangesAsync();
                }
            };

            return await Process.RunInTransactionAsync(action, context);
        }

        public async Task<ProcessResult<IdeaViewModel>> GetDetailAsync(long ideaId)
        {
            Func<Task<IdeaViewModel>> action = async () =>
            {
                var idea = await context.Ideas.Include(x => x.CreatorUser)
                                              .Include(x => x.Favorites)
                                              .Where(x => x.Id == ideaId && (!x.IsDeleted || x.CreatorId == CurrentUser.Id))
                                              .SingleAsync();
                var currentUserLike = await context.relIdeaLikes.Where(x => x.IdeaId == ideaId && x.CreatorId == CurrentUser.Id)
                                                                .Select(x => x.Vote)
                                                                .SingleOrDefaultAsync();
                var ideaTags = await context.relIdeaTags.Include(x => x.Tag)
                                                        .Where(x => x.IdeaId == ideaId)
                                                        .Select(x => new TagViewModel 
                                                        {
                                                            Id = x.Tag.Id,
                                                            Name = x.Tag.Name
                                                        })
                                                        .ToListAsync();

                idea.ViewCount++;
                await context.SaveChangesAsync();

                var ideaDetails = new IdeaViewModel
                                  {
                                      Id = idea.Id,
                                      Title = idea.Title,
                                      Article = idea.Article,
                                      Tags = ideaTags,
                                      Creator = new CreatorViewModel()
                                      {
                                          Id = idea.CreatorId,
                                          Name = idea.CreatorUser.UserName,
                                          AvatarURL = String.IsNullOrEmpty(idea.CreatorUser.AvatarURL) ? "person.png" : idea.CreatorUser.AvatarURL
                                      },
                                      CreatedDate = idea.CreateDate,
                                      CurrentUserLike = currentUserLike,
                                      CurrentUserIsFavorited = idea.Favorites.Where(x => x.CreatorId == CurrentUser.Id)
                                                                             .Select(x => x.Value).SingleOrDefault(),
                                      LikeCount = idea.LikeCount,
                                      ViewCount = idea.ViewCount,
                                      CommentCount = idea.CommentCount,
                                      IsDeleted = idea.IsDeleted
                                  };

                return ideaDetails;
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<List<IdeaViewModel>>> GetListAsync(FilterViewModel model)
        {
            Func<Task<List<IdeaViewModel>>> action = async () => 
            {
                IQueryable<Idea> result = context.Ideas.Include(x => x.Tags)
                                                       .Include(x => x.Favorites)
                                                       .Where(x => !x.IsDeleted || x.CreatorId == CurrentUser.Id);
                var takeSize = model.TakeSize ?? 10;

                if (model.TagId != null)
                    result = result.Where(x => x.Tags.Select(t => t.TagId).Contains((long)model.TagId));
                
                switch (model.Filter)
                {
                    case Filter.Best:
                        result = result.OrderByDescending(x => x.LikeCount)
                                       .ThenBy(x => x.Id);
                        // find data in Best ordered data after the defined LastIdeaId
                        if (model.LastIdeaId != null)
                        {
                            var LastLikeCount = context.Ideas.Single(x => x.Id == model.LastIdeaId).LikeCount;
                            result = result.Where(x => x.LikeCount < LastLikeCount || (x.LikeCount == LastLikeCount && x.Id > model.LastIdeaId));                 
                        }
                        break;
                    case Filter.Hot:
                        var now = DateTime.Now;
                        result = result.OrderByDescending(x => x.LikeCount / (now - x.CreateDate).TotalSeconds)
                                       .ThenBy(x => x.Id);
                        // Also, in LikeRate ordered data find data after the defined LastIdeaId
                        // LikeRate = LikeCount / Milliseconds(since the idea was posted)
                        // TODO: DateTime.Now generate duplicates
                        if (model.LastIdeaId != null)
                        {
                            var LastIdea = await context.Ideas.SingleAsync(x => x.Id == model.LastIdeaId);
                            var LastLikeRate = LastIdea.LikeCount / (now - LastIdea.CreateDate).TotalSeconds;
                            result = result.Where(x => (x.LikeCount / (now - x.CreateDate).TotalSeconds) < LastLikeRate || ((x.LikeCount / x.CreateDate.Millisecond) == LastLikeRate && x.Id > model.LastIdeaId));
                        }
                        break;
                    case Filter.Recent:
                        result = result.OrderByDescending(x => x.Id);
                        if (model.LastIdeaId != null)
                            result = result.Where(x => x.Id < model.LastIdeaId);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("filter value is not valid");
                }

                switch (model.Period)
                {
                    case Period.Day:
                        result = result.Where(x => x.CreateDate > DateTime.Now.AddDays(-1));
                        break;
                    case Period.Week:
                        result = result.Where(x => x.CreateDate > DateTime.Now.AddDays(-7));
                        break;
                    case Period.Month:
                        result = result.Where(x => x.CreateDate > DateTime.Now.AddDays(-31));
                        break;
                    case Period.AllTime:
                        break;
                }

                var ideaList = await result.Include(x => x.Likes)
                                           .Include(x => x.Comments)
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
                        AvatarURL =  String.IsNullOrEmpty(x.CreatorUser.AvatarURL) ? "person.png" : x.CreatorUser.AvatarURL
                    },
                    CreatedDate = x.CreateDate,
                    CurrentUserLike = x.Likes.Where(l => l.CreatorId == CurrentUser.Id)
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

        public async Task<ProcessResult<List<IdeaViewModel>>> SearchListAsync(string searchValue, long? lastIdeaId)
        {
            Func<Task<List<IdeaViewModel>>> action = async () => 
            {
                IQueryable<Idea> result = context.Ideas.Include(x => x.Tags)
                                                       .Include(x => x.Favorites)
                                                       .Where(x => x.Title.Contains(searchValue) || x.Article.Contains(searchValue))
                                                       .Where(x => !x.IsDeleted || x.CreatorId == CurrentUser.Id)
                                                       .OrderByDescending(x => x.Id);;

                if (lastIdeaId != null)
                    result = result.Where(x => x.Id < lastIdeaId);

                var ideaList = await result.Include(x => x.Likes)
                                           .Include(x => x.Comments)
                                           .Take(10)
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
                    CurrentUserLike = x.Likes.Where(l => l.CreatorId == CurrentUser.Id)
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

        public async Task<ProcessResult> LikeAsync(LikeViewModel model)
        {
            Func<Task> action = async () => 
            {
                var idea = await context.Ideas.Where(x => x.Id == model.ObjectId).SingleAsync();
                if (idea.CreatorId == CurrentUser.Id)
                    throw new InvalidOperationException("The owner cannot like own idea");
                
                var likeEntity = await GetOrCreateEntityAsync(context.relIdeaLikes, 
                                                   x => x.IdeaId == model.ObjectId && x.CreatorId == CurrentUser.Id);
                var like = likeEntity.result;
                if (likeEntity.isCreated)
                    like.IdeaId = model.ObjectId;

                // count the difference between the last vote and the current one
                var likeDifference = model.Vote - like.Vote;
                idea.LikeCount += likeDifference;
                like.Vote = model.Vote;

                // change user's rating
                var user = await this.userManager.FindByIdAsync(idea.CreatorId.ToString());
                user.Rating += likeDifference * 10;

                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }


        public async Task<ProcessResult> AddToFavoritesAsync(long ideaId)
        {
            Func<Task> action = async () =>
            {
                var favoriteEntity = await GetOrCreateEntityAsync(context.relIdeaFavorites,
                                                        x => x.IdeaId == ideaId && x.CreatorId == CurrentUser.Id);
                var favorite = favoriteEntity.result;
                if (favoriteEntity.isCreated)
                    favorite.IdeaId = ideaId;

                favorite.Value = !favorite.Value;
        
                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> RemoveOrRestoreAsync(long ideaId)
        {
            Func<Task> action = async () =>
            {
                var idea = await context.Ideas.Where(x => x.Id == ideaId).SingleAsync();
                if (idea.CreatorId != CurrentUser.Id)
                    throw new InvalidOperationException("Only creator can remove/restore the idea");
                
                //string resultMessage = idea.IsDeleted ? "The idea was successfully restored!" : "The idea was successfully removed!";
                idea.IsDeleted = !idea.IsDeleted;

                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }
    }
}