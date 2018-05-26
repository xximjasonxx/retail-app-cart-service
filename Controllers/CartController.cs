using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CartApi.Caching;
using CartApi.Common;
using CartApi.Requests;
using CartApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CartApi.Controllers
{
    [Route("api/cart")]
    public class CartController : Controller
    {
        private readonly ICartService cartService;
        private readonly IUserDataContext userDataContext;

        public CartController(ICartService pCartService, IUserDataContext pContext)
        {
            this.cartService = pCartService;
            this.userDataContext = pContext;
        }

        [HttpPost]
        [Route("item")]
        [RequireUser]
        public async Task<IActionResult> AddItem([FromBody] AddItemRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await this.cartService.AddItemWithQuantityAsync(
                    request.ProductId, request.Quantity, this.userDataContext.CurrentUser.UserId);
                return Accepted();
            }
            catch (ArgumentException)
            {
                return BadRequest(new { Message = "Quantity is invalid - must be positive" });
            }
            catch (BadProductRequestException)
            {
                return NotFound(new { Message = "The desired product is not available" });
            }
        }

        [HttpGet]
        [RequireUser]
        public IActionResult GetCart()
        {
            return Ok(this.cartService.GetShoppingCartForUser(this.userDataContext.CurrentUser.UserId));
        }

        [HttpDelete]
        [RequireUser]
        [Route("item/{productId}")]
        public IActionResult DeleteFromCart(string productId)
        {
            try
            {
                this.cartService.DeleteFromCart(productId, this.userDataContext.CurrentUser.UserId);
                return Accepted();
            }
            catch (CartNotFoundException)
            {
                return NotFound(new { Message = "Your user does not have a shopping cart" });
            }
        }

        [HttpPost]
        [RequireUser]
        [Route("item/{productId}")]
        public IActionResult UpdateItemQuantity(string productId, [FromBody] NewItemQuantityRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                this.cartService.UpdateProductQuantity(productId, request.Quantity, this.userDataContext.CurrentUser.UserId);
                return Accepted();
            }
            catch (CartNotFoundException)
            {
                return NotFound(new { Message = "The current user does not have a shopping cart" });
            }
            catch (BadProductRequestException)
            {
                return NotFound(new { Message = "The desired product could not be found in the shopping cart" });
            }
        }
    }
}
