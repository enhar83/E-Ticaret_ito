using Entity;

namespace Presentation.Models
{
    public class CompareViewModel
    {
        public required Product SelectedProduct { get; set; }
        public List<Product> SelectedCategoryProducts { get; set; } = new List<Product>();
        public Product? ComparedProduct { get; set; }
    }
}
