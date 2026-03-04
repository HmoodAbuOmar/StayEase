namespace LibrarySystem.DAL.Models
{
    public class Category : BaseModel
    {
        public List<Book>? Books { get; set; }
        public List<CategoryTranlsation>? Translations { get; set; }
    }
}
