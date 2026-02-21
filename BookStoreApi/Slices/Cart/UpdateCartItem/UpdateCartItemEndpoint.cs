using BookStoreApi.Data;
using BookStoreApi.Slices.Cart.Common;

namespace BookStoreApi.Slices.Cart.UpdateCartItem
{
    public static class UpdateCartItemEndpoint
    {
        public static void MapUpdateCartItemEndpoint(this WebApplication app)
        {
            app.MapPatch("/api/cart/items/{bookId:guid}", async (Guid bookId, UpdateCartItemRequest request, HttpContext httpContext, AppDbContext context) =>
            {
                if (request.Quantity <= 0)
                {
                    return Results.BadRequest("Quantity must be greater than 0.");
                }

                var cart = CartSessionStore.GetCart(httpContext.Session);
                var item = cart.FirstOrDefault(x => x.BookId == bookId);
                if (item is null)
                {
                    return Results.NotFound("Cart item not found.");
                }

                item.Quantity = request.Quantity;
                CartSessionStore.SaveCart(httpContext.Session, cart);

                var response = await CartResponseFactory.BuildAsync(cart, context);
                return Results.Ok(response);
            });
        }
    }
}
