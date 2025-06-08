using BookStoreApi.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Slices.Reviews.UpdateReview
{
    public static class UpdateReviewEndpoint
    {
        public static void MapUpdateReviewEndpoint(this WebApplication app)
        {
            app.MapPut("/api/reviews/{id:guid}", async (Guid id, UpdateReviewRequest request, AppDbContext context, IValidator<UpdateReviewRequest> validator) =>
            {
                // validation 
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                    return Results.BadRequest(string.Join(", ", validationResult.Errors));

                var review = await context.Reviews
                    .FirstOrDefaultAsync(r => r.Id == id);
                if (review is null)
                    return Results.NotFound();

                review.Content = request.Content;
                review.Rating = request.Rating;
                await context.SaveChangesAsync();

                return Results.NoContent();
            });
        }
    }
}
