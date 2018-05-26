
using System.Collections.Generic;
using System.Threading.Tasks;
using CartApi.Models;

namespace CartApi.Services
{
    public interface ICartService
    {
        Task<bool> AddItemWithQuantityAsync(string productId, int quantity, string userId);

        List<CartItem> GetShoppingCartForUser(string userId);

        bool DeleteFromCart(string productId, string userId);

        bool UpdateProductQuantity(string productId, int newQuantity, string userId);
    }
}