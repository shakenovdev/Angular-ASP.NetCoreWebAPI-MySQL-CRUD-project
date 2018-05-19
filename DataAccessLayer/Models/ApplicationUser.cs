using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models
{
    public class ApplicationUser : IdentityUser<long>
    {
        public int Rating { get; set; }
        public string AvatarURL { get; set; }
        
        public ICollection<Idea> Ideas { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Tag> Tags { get; set; }

        public ICollection<relIdeaTag> relIdeaTags { get; set; }

        public ICollection<relIdeaFavorite> relIdeaFavorites { get; set; }

        public ICollection<relIdeaLike> relIdeaLikes { get; set; }

        public ICollection<relCommentLike> relCommentLikes { get; set; }
    }
}