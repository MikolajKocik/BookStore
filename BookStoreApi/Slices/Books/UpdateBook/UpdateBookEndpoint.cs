using BookStoreApi.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Slices.Books.UpdateBook
{
    public static class UpdateBookEndpoint
    {
        public static void MapUpdateBookEndpoint(this WebApplication app)
        {
            app.MapPut("/api/books/{id:guid}", async (Guid id, UpdateBookRequest request, AppDbContext context, IValidator<UpdateBookRequest> validator) =>
            {
                // validation
                var validationResult = validator.Validate(request);
                if(!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                // get book
                var book = await context.Books
                    .FirstOrDefaultAsync(b => b.Id == id);
                if(book is null)
                {
                    return Results.NotFound();
                }

                // update fields
                book.Title = request.Title;
                book.Author = request.Author;
                book.Price = request.Price;

                await context.SaveChangesAsync();

                return Results.NoContent();
            });
        }
    }
}
