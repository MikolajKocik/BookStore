using BookStoreApi.Slices.Cart.Common;

namespace BookStoreApi.Slices.Cart.ClearCart
{
    public static class ClearCartEndpoint
    {
        public static void MapClearCartEndpoint(this WebApplication app)
        {
            app.MapDelete("/api/cart", (HttpContext httpContext) =>
            {
                CartSessionStore.Clear(httpContext.Session);
                return Results.NoContent();
            });
        }
    }
}
