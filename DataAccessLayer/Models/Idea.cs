using DataAccessLayer.Models;
using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    public class Idea : BaseModel
    {
        public string Title { get; set; }
        public string Article { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        [NotMapped]
        public int CommentCount { get; set; }
        public Language Language { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<relIdeaTag> Tags { get; set; }
        public ICollection<relIdeaLike> Likes { get; set; }
        public ICollection<relIdeaFavorite> Favorites { get; set; }
    }
}