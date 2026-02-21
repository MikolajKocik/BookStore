using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace BookStoreApi.Slices.Cart.Common
{
    public static class CartSessionStore
    {
        private const string CartSessionKey = "cart";

        public static List<CartItemSession> GetCart(ISession session)
        {
            var json = session.GetString(CartSessionKey);
            if (string.IsNullOrWhiteSpace(json))
            {
                return [];
            }

            return JsonSerializer.Deserialize<List<CartItemSession>>(json) ?? [];
        }

        public static void SaveCart(ISession session, List<CartItemSession> items)
        {
            session.SetString(CartSessionKey, JsonSerializer.Serialize(items));
        }

        public static void Clear(ISession session)
        {
            session.Remove(CartSessionKey);
        }
    }

    public class CartItemSession
    {
        public Guid BookId { get; set; }
        public int Quantity { get; set; }
    }
}
