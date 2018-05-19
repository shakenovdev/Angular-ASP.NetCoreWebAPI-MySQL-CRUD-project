using DataAccessLayer;
using DataAccessLayer.Models;
using WebApi.Models;
using WebApi.Services.Interfaces;
using WebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace WebApi.Services
{
    public class CommentService : BaseService, ICommentService
    {
        public CommentService(UserManager<ApplicationUser> userManager,
                              IHttpContextAccessor contextAccessor, 
                              ApplicationDbContext context) 
            : base(userManager, contextAccessor, context)
        {
        }

        public async Task<ProcessResult> CreateOrUpdateAsync(CommentNewViewModel model)
        {
            Func<Task> action = async () =>
            {
                var commentEntity = await GetOrCreateEntityAsync(context.Comments, x => x.Id == model.Id);
                var comment = commentEntity.result;
                if (!commentEntity.isCreated && CurrentUser.Id != comment.CreatorId)
                    throw new InvalidOperationException("You're not owner of requested comment");

                comment.IdeaId = model.IdeaId;
                comment.Message = model.Message;
                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<List<CommentViewModel>>> GetListAsync(long ideaId)
        {
            Func<Task<List<CommentViewModel>>> action = async () =>
            {
                var result = await context.Comments.Include(x => x.CreatorUser)
                                                   .Include(x => x.Likes)
                                                   .Where(x => x.IdeaId == ideaId && (!x.IsDeleted || x.CreatorId == CurrentUser.Id))
                                                   .Select(x => new CommentViewModel(){
                                                       Id = x.Id,
                                                       Message = x.Message,
                                                       Creator = new CreatorViewModel()
                                                       {
                                                           Id = x.CreatorId,
                                                           Name = x.CreatorUser.UserName,
                                                           AvatarURL = String.IsNullOrEmpty(x.CreatorUser.AvatarURL) ? "person.png" : x.CreatorUser.AvatarURL
                                                       },
                                                       CurrentUserLike = x.Likes.Where(l => l.CreatorId == CurrentUser.Id)
                                                                                .Select(l => l.Vote).SingleOrDefault(),
                                                       LikeCount = x.LikeCount,
                                                       CreatedDate = x.CreateDate,
                                                       IsDeleted = x.IsDeleted
                                                   }).OrderByDescending(x => x.LikeCount)
                                                   .ThenByDescending(x => x.CreatedDate)
                                                   .ToListAsync();

                return result;
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> LikeAsync(LikeViewModel model)
        {
            Func<Task> action = async () =>
            {
                var comment = await context.Comments.Where(x => x.Id == model.ObjectId).SingleAsync();
                if (comment.CreatorId == CurrentUser.Id)
                    throw new InvalidOperationException("The owner cannot like own comment");

                var likeEntity = await GetOrCreateEntityAsync(context.relCommentLikes, 
                                                    x => x.CommentId == model.ObjectId && x.CreatorId == CurrentUser.Id);
                var like = likeEntity.result;
                if (likeEntity.isCreated)
                    like.CommentId = model.ObjectId;
                
                // count the difference between the last vote and the current one
                var likeDifference = model.Vote - like.Vote;
                comment.LikeCount += likeDifference;
                like.Vote = model.Vote;

                // change user's rating
                var user = await this.userManager.FindByIdAsync(comment.CreatorId.ToString());
                user.Rating += likeDifference;

                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> RemoveOrRestoreAsync(long id)
        {
            Func<Task> action = async () =>
            {
                var comment = await context.Comments.Where(x => x.Id == id).SingleAsync();
                if (comment.CreatorId != CurrentUser.Id)
                    throw new InvalidOperationException("Only creator can remove/restore the comment");

                comment.IsDeleted = !comment.IsDeleted;

                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }
    }
}