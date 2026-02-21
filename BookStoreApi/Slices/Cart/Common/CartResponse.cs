namespace BookStoreApi.Slices.Cart.Common
{
    public class CartResponse
    {
        public List<CartItemResponse> Items { get; set; } = [];
        public decimal TotalPrice { get; set; }
    }

    public class CartItemResponse
    {
        public Guid BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string CoverImagePath { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal { get; set; }
    }
}
