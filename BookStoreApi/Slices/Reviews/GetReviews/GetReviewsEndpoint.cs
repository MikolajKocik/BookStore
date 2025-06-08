using BookStoreApi.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Slices.Reviews.GetReviews
{
    public static class GetReviewsEndpoint
    {
        public static void MapGetReviewsEndopint(this WebApplication app)
        {
            app.MapPost("/api/books/{bookId:guid}/reviews", async (Guid bookId, AppDbContext context) =>
            {
                var bookExists = await context.Books.AnyAsync(x => x.Id == bookId);
                if(!bookExists)
                {
                    return Results.NotFound($"Book with id {bookId} not found");
                }

                // get review
                var reviews = await context.Reviews
                    .Where(x => x.BookId == bookId)
                    .Select(r => new GetReviewsResponse
                    {
                        Id = r.Id,
                        Content = r.Content,
                        Rating = r.Rating
                    }).ToListAsync();

                return Results.Ok(reviews);
            });
        }
    }
}
