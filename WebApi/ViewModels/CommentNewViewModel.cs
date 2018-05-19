using System.ComponentModel.DataAnnotations;

namespace WebApi.ViewModels
{
    public class CommentNewViewModel
    {
        public long Id { get; set; } = 0;
        public long IdeaId { get; set; }
        [Required]
        [StringLength(255, MinimumLength=3)]
        public string Message { get; set; }
    }
}