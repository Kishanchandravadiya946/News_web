namespace NEWS_App.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishedDate { get; set; }
        public int CategoryId { get; set; } // Foreign Key
        public bool IsPublished { get; set; }
        public string ImageUrl { get; set; } // URL or path to the article image
        public string VideoUrl { get; set; } // URL or path to the article video

        public int LikeCount { get; set; } 

        // Navigation properties
        public virtual Category Category { get; set; } // Reference to the category
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } // Article can have multiple comments
        public virtual ICollection<Like> Likes { get; set; } // Article can have multiple likes
    }

}
