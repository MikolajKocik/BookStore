namespace BookStoreApi.Slices.Books.GetBook
{
    public class GetBookResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string Author { get; set; } = default!;
        public decimal Price { get; set; }
        public string CoverImagePath { get; set; } = string.Empty;

        public List<ReviewDto> Reviews { get; set; } = new();

        public class ReviewDto
        {
            public Guid Id { get; set; }
            public string Content { get; set; } = default!;
            public int Rating { get; set; }
        }
    }
}
