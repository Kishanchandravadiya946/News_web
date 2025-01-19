namespace NEWS_App.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int ArticleId { get; set; } // Foreign Key
        public int UserId { get; set; } // Foreign Key

        // Navigation properties
        public virtual Article Article { get; set; } // Reference to the article
        public virtual User User { get; set; } // Reference to the user
    }

}
