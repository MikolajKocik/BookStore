namespace BookStoreApi.Models
{
    public class Review
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = default!;
        public int Rating { get; set; }

        // relation with book

        public Book? Book { get; set; }
        public Guid BookId { get; set; }
    }
}
