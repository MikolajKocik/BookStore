using BookStoreApi.Data;
using BookStoreApi.Slices.Cart.Common;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Slices.Cart.AddCartItem
{
    public static class AddCartItemEndpoint
    {
        public static void MapAddCartItemEndpoint(this WebApplication app)
        {
            app.MapPost("/api/cart/items", async (AddCartItemRequest request, HttpContext httpContext, AppDbContext context) =>
            {
                if (request.BookId == Guid.Empty)
                {
                    return Results.BadRequest("BookId is required.");
                }

                if (request.Quantity <= 0)
                {
                    return Results.BadRequest("Quantity must be greater than 0.");
                }

                var bookExists = await context.Books.AnyAsync(b => b.Id == request.BookId);
                if (!bookExists)
                {
                    return Results.NotFound($"Book with id {request.BookId} not found.");
                }

                var cart = CartSessionStore.GetCart(httpContext.Session);
                var existing = cart.FirstOrDefault(x => x.BookId == request.BookId);
                if (existing is null)
                {
                    cart.Add(new CartItemSession
                    {
                        BookId = request.BookId,
                        Quantity = request.Quantity
                    });
                }
                else
                {
                    existing.Quantity += request.Quantity;
                }

                CartSessionStore.SaveCart(httpContext.Session, cart);
                var response = await CartResponseFactory.BuildAsync(cart, context);
                return Results.Ok(response);
            });
        }
    }
}
