using System.ComponentModel.DataAnnotations;

namespace NEWS_App.Models
{
    public class Notification
    {
        public int Id { get; set; } // Unique identifier for the notification

        // Foreign key for User
        public int UserId { get; set; }
        public virtual User User { get; set; } // Navigation property

        // Foreign key for Article
        public int ArticleId { get; set; }
        public virtual Article Article { get; set; } // Navigation property

        public DateTime CreatedAt { get; set; } // Date and time when the notification was created
    }
}
