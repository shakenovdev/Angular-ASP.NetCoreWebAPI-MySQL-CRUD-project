using WebApi.Models.Enums;
using System;

namespace WebApi.ViewModels
{
    public class ProfileFilterViewModel
    {
        public Kind Kind { get; set; }
        public string UserName { get; set; }
        public long? LastIdeaId { get; set; }
        public int? TakeSize { get; set; }
    }
}