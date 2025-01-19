using System.ComponentModel.DataAnnotations;

namespace NEWS_App.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string PasswordHash { get; set; } // Store hashed passwords

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool notification { get; set; }

        // Navigation properties
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } // User can have multiple comments
        public virtual ICollection<Like> Likes { get; set; } // User can have multiple likes
    }

}
