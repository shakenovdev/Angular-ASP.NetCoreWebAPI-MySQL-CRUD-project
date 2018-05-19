namespace DataAccessLayer.Models
{
    public class relIdeaFavorite : BaseModel
    {
        public long IdeaId { get; set; }
        public bool Value { get; set; }
        public Idea Idea { get; set; }
    }
}