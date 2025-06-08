using BookStoreApi.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Slices.Books.UploadCoverImage
{
    public static class UploadCoverImageEndpoint
    {
        public static void MapUploadCoverImageEndpoint(this WebApplication app)
        {
            app.MapPost("/api/books/{id:guid}/cover", async (Guid id, IFormFile file, AppDbContext context) =>
            {
                // book exist
                var book = await context.Books
                    .FirstOrDefaultAsync(b => b.Id == id);
                if (book is null)
                {
                    return Results.NotFound();
                }

                // check file if exists
                if (file is null || file.Length == 0)
                {
                    return Results.BadRequest("No file uploaded");
                }

                // file path
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "covers");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                // save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                book.CoverImagePath = $"/covers/{fileName}";
                await context.SaveChangesAsync();

                return Results.Ok(new { book.Id, CoverImagePath = book.CoverImagePath });
            });
        }
    }
}
