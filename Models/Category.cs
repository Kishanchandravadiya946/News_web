namespace NEWS_App.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation property
        public virtual ICollection<Article> Articles { get; set; } // Category can have multiple articles
    }

}
