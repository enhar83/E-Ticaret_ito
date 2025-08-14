namespace Presentation.Models.CartViewModels
{
    public class CartViewModel //Sepetin hepsini kapsayacak
    {
        public List<CartItemViewModel> Items { get; set; } = new();
        public decimal GrandTotal => Items.Sum(i => i.TotalPrice);
    }
}
