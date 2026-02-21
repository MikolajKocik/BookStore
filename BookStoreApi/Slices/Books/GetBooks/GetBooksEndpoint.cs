using BookStoreApi.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Slices.Books.GetBooks
{
    public static class GetBooksEndpoint
    {
        public static void MapGetBooksEndpoint(this WebApplication app)
        {
            app.MapGet("/api/books", async (
                int page,
                int pageSize,
                string? author,
                string? title,
                decimal? minPrice,
                decimal? maxPrice,
                AppDbContext context) =>
            {

                if (page <= 0) page = 1;
                if (pageSize <= 0) pageSize = 10;

                var query = context.Books.AsQueryable();

                // filtration

                if(!string.IsNullOrWhiteSpace(author))
                {
                    query = query.Where(b => b.Author.Contains(author));
                }
                
                if(!string.IsNullOrWhiteSpace(title))
                {
                    query = query.Where(b => b.Title.Contains(title));
                }

                if(minPrice.HasValue)
                {
                    query = query.Where(b => b.Price >= minPrice.Value);
                }

                if(maxPrice.HasValue)
                {
                    query = query.Where(b => b.Price <= maxPrice.Value);
                }

                // count books
                var totalCount = await query.CountAsync();

                // pagination
                var books = await query
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
