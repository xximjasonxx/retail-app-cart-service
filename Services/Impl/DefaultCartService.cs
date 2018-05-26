
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CartApi.Caching;
using CartApi.Common;
using CartApi.Models;

namespace CartApi.Services.Impl
{
    // TODO: This is an ideal use care for the repository pattern
    public class DefaultCartService : ICartService
    {
        private readonly ICacheClient cartCacheClient;
        private readonly IProductService productService;

        public DefaultCartService(ICacheClient cacheClient, IProductService pService)
        {
            this.cartCacheClient = cacheClient;
            this.productService = pService;
        }

        // there are potential concurrency risks here
        public async Task<bool> AddItemWithQuantityAsync(string productId, int quantity, string userId)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity cannot be less than 1");
            }

            // first - do we have an existing cat in cache
            var cartKey = GenerateCartKey(userId);
            var shoppingCart = this.cartCacheClient.GetValue<List<CartItem>>(cartKey);
            if (shoppingCart == null)
            {
                shoppingCart = new List<CartItem>();
            }

            // second - see if the product already exists in the shopping cart
            // if it does - we are replacing the quantity
            var item = shoppingCart.FirstOrDefault(x => x.ProductId == productId);
            if (item == null)
            {
                // if the item is not found, we need to look up the info before adding it
                var product = await this.productService.GetProductAsync(productId);
                if (product == null)
                {
                    throw new BadProductRequestException();
                }

                // todo: check inventory
                // add the item to the cart
                shoppingCart.Add(new CartItem
                {
                    ProductId = productId,
                    Quanity = quantity,
                    ProductName = product.Name,
                    Price = product.Price
                });
            }
            else
            {
                item.Quanity = quantity;
            }

            // put the item in the cart and overwrite the previous cart
            this.cartCacheClient.SetValue(cartKey, shoppingCart, TimeSpan.FromDays(1));

            return true;
        }

        public bool DeleteFromCart(string productId, string userId)
        {
            var cartKey = GenerateCartKey(userId);
            var shoppingCart = this.cartCacheClient.GetValue<List<CartItem>>(cartKey);
            if (shoppingCart == null)
            {
                throw new CartNotFoundException(cartKey);
            }

            shoppingCart = shoppingCart.Where(x => x.ProductId != productId).ToList();
            this.cartCacheClient.SetValue(cartKey, shoppingCart, TimeSpan.FromDays(1));

            return true;
        }

        public List<CartItem> GetShoppingCartForUser(string userId)
        {
            var cartKey = GenerateCartKey(userId);
            var shoppingCart = this.cartCacheClient.GetValue<List<CartItem>>(cartKey);
            if (shoppingCart == null)
            {
                shoppingCart = new List<CartItem>();
            }

            return shoppingCart;
        }

        public bool UpdateProductQuantity(string productId, int newQuantity, string userId)
        {
            if (newQuantity <= 0)
            {
                throw new ArgumentException("Quantity cannot be less than 1");
            }

            var cartKey = GenerateCartKey(userId);
            var shoppingCart = this.cartCacheClient.GetValue<List<CartItem>>(cartKey);
            if (shoppingCart == null)
            {
                throw new CartNotFoundException(cartKey);
            }

            var cartItem = shoppingCart.FirstOrDefault(x => x.ProductId == productId);
            if (cartItem == null)
            {
                throw new BadProductRequestException();
            }

            cartItem.Quanity = newQuantity;
            shoppingCart = shoppingCart.Where(x => x.ProductId != productId).Concat(new[] { cartItem }).ToList();
            this.cartCacheClient.SetValue(cartKey, shoppingCart, TimeSpan.FromDays(1));

            return true;
        }

        string GenerateCartKey(string userId)
        {
            return $"cart_{userId}";
        }
    }
}