using BookStoreApi.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Slices.Reviews.DeleteReview
{
    public static class DeleteReviewEndpoint
    {
        public static void MapDeleteReviewEndpoint(this WebApplication app)
        {
            app.MapDelete("/api/reviews/{id:guid}", async (Guid id, AppDbContext context) =>
            {
                var review = await context.Reviews.FirstOrDefaultAsync(r => r.Id == id);
                if (review is null)
                    return Results.NotFound();

                context.Reviews.Remove(review);
                await context.SaveChangesAsync();

                return Results.NoContent();
            });
        }
    } 
}
