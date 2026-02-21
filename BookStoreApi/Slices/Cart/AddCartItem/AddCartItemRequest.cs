namespace BookStoreApi.Slices.Cart.AddCartItem
{
    public class AddCartItemRequest
    {
        public Guid BookId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
