using BookStoreApi.Data;
using BookStoreApi.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Slices.Reviews.CreateReview
{
    public static class CreateReviewEndpoint
    {
        public static void MapCreateReviewEndpoint(this WebApplication app)
        {
            app.MapPost("/api/reviews", async (CreateReviewRequest request, AppDbContext context, IValidator<CreateReviewRequest> validator) =>
            {
                // validation
                var validationResult = validator.Validate(request);
                if(!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                // if book exists
                var bookExists = await context.Books
                    .AnyAsync(b => b.Id == request.BookId);
                if(!bookExists)
                {
                    return Results.NotFound($"Book with id {request.BookId} not found.");
                }

                // create review
                var review = new Review
                {
                    Id = Guid.NewGuid(),
                    Content = request.Content,
                    Rating = request.Rating,
                    BookId = request.BookId
                };

                context.Reviews.Add(review);
                await context.SaveChangesAsync();

                return Results.Created($"/api/reviews/{review.Id}", review);
            });
        }
    }
}
