using System;
using System.Collections.Generic;
using DataAccessLayer.Enums;

namespace WebApi.ViewModels
{
    public class IdeaViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Article { get; set; }
        public List<TagViewModel> Tags { get; set; }
        public CreatorViewModel Creator { get; set; }
        public DateTime CreatedDate { get; set; }
        public Vote CurrentUserLike { get; set; }
        public bool CurrentUserIsFavorited { get; set; }
        public int LikeCount { get; set; }
        public int ViewCount { get; set; }
        public int CommentCount { get; set; }
        public bool IsDeleted { get; set; }
    }
}