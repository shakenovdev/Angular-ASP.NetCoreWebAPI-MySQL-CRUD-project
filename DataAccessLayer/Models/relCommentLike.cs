using DataAccessLayer.Enums;
using DataAccessLayer.Models;
using System;

namespace DataAccessLayer.Models
{
    public class relCommentLike : BaseModel
    {
        public long CommentId { get; set; }
        public Vote Vote { get; set; }
        public Comment Comment { get; set; }
    }
}