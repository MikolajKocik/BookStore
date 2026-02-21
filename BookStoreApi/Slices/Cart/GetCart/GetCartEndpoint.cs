using BookStoreApi.Data;
using BookStoreApi.Slices.Cart.Common;

namespace BookStoreApi.Slices.Cart.GetCart
{
    public static class GetCartEndpoint
    {
        public static void MapGetCartEndpoint(this WebApplication app)
        {
            app.MapGet("/api/cart", async (HttpContext httpContext, AppDbContext context) =>
            {
                var cart = CartSessionStore.GetCart(httpContext.Session);
                var response = await CartResponseFactory.BuildAsync(cart, context);

                return Results.Ok(response);
            });
        }
    }
}
