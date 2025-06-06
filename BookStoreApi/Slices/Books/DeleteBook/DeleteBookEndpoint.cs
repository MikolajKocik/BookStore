using BookStoreApi.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Slices.Books.DeleteBook
{
    public static class DeleteBookEndpoint
    {
        public static void MapDeleteBookEndpoint(this WebApplication app)
        {
            app.MapDelete("/api/books/{id:guid}", async (Guid id, AppDbContext context) =>
            {
                var book = await context.Books
                    .FirstOrDefaultAsync(b => b.Id == id);
                if(book is null)
                {
                    return Results.NotFound();
                }

                context.Books.Remove(book);
                await context.SaveChangesAsync();

                return Results.NoContent();
            });
        }
    }
}
