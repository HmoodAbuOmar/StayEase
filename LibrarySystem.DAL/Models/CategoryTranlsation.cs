namespace LibrarySystem.DAL.Models
{
    public class CategoryTranlsation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Language { get; set; } = "en";
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
