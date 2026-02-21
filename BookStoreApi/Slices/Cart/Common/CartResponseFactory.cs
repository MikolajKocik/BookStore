using BookStoreApi.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Slices.Cart.Common
{
    public static class CartResponseFactory
    {
        public static async Task<CartResponse> BuildAsync(List<CartItemSession> sessionItems, AppDbContext context)
        {
            if (sessionItems.Count == 0)
            {
                return new CartResponse();
            }

            var bookIds = sessionItems.Select(x => x.BookId).ToList();
            var books = await context.Books
                .Where(b => bookIds.Contains(b.Id))
                .ToDictionaryAsync(b => b.Id);

            var responseItems = new List<CartItemResponse>();
            foreach (var item in sessionItems)
            {
                if (!books.TryGetValue(item.BookId, out var book))
                {
                    continue;
                }

                responseItems.Add(new CartItemResponse
                {
                    BookId = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    CoverImagePath = book.CoverImagePath,
                    UnitPrice = book.Price,
                    Quantity = item.Quantity,
                    LineTotal = book.Price * item.Quantity
                });
            }

            return new CartResponse
            {
                Items = responseItems,
                TotalPrice = responseItems.Sum(x => x.LineTotal)
            };
        }
    }
}
