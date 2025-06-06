using BookStoreApi.Data;
using BookStoreApi.Models;
using FluentValidation;

namespace BookStoreApi.Slices.Books.CreateBook
{
    public static class CreateBookEndpoint
    {
        public static void MapCreateBookEndpoint(this WebApplication app)
        {
            app.MapPost("/api/books", async (CreateBookRequest request, AppDbContext context, IValidator<CreateBookRequest> validator) =>
            {
                // validation
                var validationResult = await validator.ValidateAsync(request);
                if(!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                // create entity
                var book = new Book
                {
                    Id = Guid.NewGuid(),
                    Title = request.Title,
                    Author = request.Author,
                    Price = request.Price
                };

                context.Books.Add(book);

                await context.SaveChangesAsync();

                // return 201
                return Results.Created($"/api/books/{book.Id}", book);
            });

        }
    }
}
