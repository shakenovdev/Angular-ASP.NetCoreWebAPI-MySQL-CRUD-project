using System;
using System.Collections.Generic;

namespace WebApi.ViewModels
{
    public class ProfileViewModel
    {
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int SharedCount { get; set; }
        public int FavoritedCount { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public int Rating { get; set; }
        public List<TagViewModel> ContributedTags { get; set; }
    }
}