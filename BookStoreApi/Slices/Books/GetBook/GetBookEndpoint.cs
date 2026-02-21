using BookStoreApi.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Slices.Books.GetBook
{
    public static class GetBookEndpoint
    {
        public static void MapGetBookEndpoint(this WebApplication app)
        {
            app.MapGet("/api/books/{id:guid}", async (Guid id, AppDbContext context) => 
            {
                // get book from db
                var book = await context.Books
                    .Include(r => r.Reviews)
                    .FirstOrDefaultAsync(b => b.Id == id);

                if(book is null)
                {
                    return Results.NotFound();
                }

                // mapping to response DTO
                var response = new GetBookResponse
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    Price = book.Price,
                    CoverImagePath = book.CoverImagePath,
                    Reviews = book.Reviews.Select(r => new GetBookResponse.ReviewDto
                    {
                        Id = r.Id,
                        Content = r.Content,
                        Rating = r.Rating
                    }).ToList()
                };

                return Results.Ok(response);
            });
        }
    }
}
