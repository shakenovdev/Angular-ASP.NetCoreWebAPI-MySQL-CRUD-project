using DataAccessLayer.Models;
using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models
{
    public class Tag : BaseModel
    {
        public string Name { get; set; }
        public Language Language { get; set; }
        public ICollection<relIdeaTag> IdeaTags { get; set; }

    }
}