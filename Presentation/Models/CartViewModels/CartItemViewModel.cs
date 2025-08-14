namespace Presentation.Models.CartViewModels
{
    public class CartItemViewModel //sepetteki sadece tek bir ürünü kapsayacak 
    {
        public Guid ProductId { get; set; }
        public required string Title { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => Price * Quantity;
        public required string ImageUrl { get; set; }
    }
}
