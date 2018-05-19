using DataAccessLayer.Enums;
using DataAccessLayer.Models;
using System;

namespace DataAccessLayer.Models
{
    public class relIdeaLike : BaseModel
    {
        public long IdeaId { get; set; }
        public Vote Vote { get; set; }
        public Idea Idea { get; set; }
    }
}