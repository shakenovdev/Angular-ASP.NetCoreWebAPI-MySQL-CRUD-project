using DataAccessLayer.Models;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models
{
    public class Comment : BaseModel
    {
        public long IdeaId { get; set; }
        public string Message { get; set; }
        public int LikeCount { get; set; }
        public Idea Idea { get; set; }
        public ICollection<relCommentLike> Likes { get; set; }
    }
}