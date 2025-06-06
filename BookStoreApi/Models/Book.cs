namespace BookStoreApi.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string Author { get; set; } = default!;
        public decimal Price { get; set; }
        public string CoverImagePath { get; set; } = default!;

        // review relation

        public List<Review> Reviews { get; set; } = new();
    }
}
