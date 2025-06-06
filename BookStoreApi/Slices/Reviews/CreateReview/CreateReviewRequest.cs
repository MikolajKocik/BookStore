namespace BookStoreApi.Slices.Reviews.CreateReview
{
    public class CreateReviewRequest
    {
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; }
        public Guid BookId { get; set; }
    }
}
