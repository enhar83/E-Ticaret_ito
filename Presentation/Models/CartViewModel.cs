namespace Presentation.Models
{
    public class CartViewModel
    {
        public Guid ProductId { get; set; }
        public required string Title { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public required string ImageUrl { get; set; }
    }
}
