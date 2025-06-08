using BookStoreApi.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Slices.Books.GetBooks
{
    public static class GetBooksEndpoint
    {
        public static void MapGetBooksEndpoint(this WebApplication app)
        {
            app.MapGet("/api/books", async (int page, int pageSize, AppDbContext context) =>
            {
                if (page <= 0) page = 1;
                if (pageSize <= 0) pageSize = 10;

                // count books
                var totalCount = context.Books.CountAsync();

                // pagination
                var books = await context.Books
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Results.Ok(new
                {
                    totalCount,
                    page,
                    pageSize,
                    books
                });
            });
        }
    }
}
