using BookStoreApi.Data;
using BookStoreApi.Slices.Cart.Common;

namespace BookStoreApi.Slices.Cart.RemoveCartItem
{
    public static class RemoveCartItemEndpoint
    {
        public static void MapRemoveCartItemEndpoint(this WebApplication app)
        {
            app.MapDelete("/api/cart/items/{bookId:guid}", async (Guid bookId, HttpContext httpContext, AppDbContext context) =>
            {
                var cart = CartSessionStore.GetCart(httpContext.Session);
                var removed = cart.RemoveAll(x => x.BookId == bookId);
                if (removed == 0)
                {
                    return Results.NotFound("Cart item not found.");
                }

                CartSessionStore.SaveCart(httpContext.Session, cart);
                var response = await CartResponseFactory.BuildAsync(cart, context);
                return Results.Ok(response);
            });
        }
    }
}
