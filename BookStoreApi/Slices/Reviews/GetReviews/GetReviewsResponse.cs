namespace BookStoreApi.Slices.Reviews.GetReviews
{
    public class GetReviewsResponse
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = default!;
        public int Rating { get; set; }
    }
}
