
namespace CartApi.Models
{
    public class CartItem
    {
        public string ProductId { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; }
        public int Quanity { get; set; }
    }
}