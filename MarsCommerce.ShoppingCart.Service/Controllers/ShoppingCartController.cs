using MarsCommerce.ShoppingCart.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MarsCommerce.Core.Models;
using MarsCommerce.Infrastructure.Repository.Migrations;
using MarsCommerce.ShoppingCart.Service.Services;
using MarsCommerce.Catalog.Service.Interfaces;

namespace MarsCommerce.ShoppingCart.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartservice;
        private readonly ILogger<ShoppingCartController> _logger;

        public ShoppingCartController(IShoppingCartService shoppingCartservice,
                                      ILogger<ShoppingCartController> logger)
        {
            _shoppingCartservice = shoppingCartservice;

            _logger = logger;
        }


        [HttpPost]
        [Route("AddItemToCart")]
        public async Task<ActionResult<Core.Models.ShoppingCart>> AddToCartItem(Core.Models.ShoppingCart shoppingCart)
        {
            if (shoppingCart.UserId != 0)
            {
                try
                {
                    Core.Models.ShoppingCart result = await _shoppingCartservice.AddItemToCart(shoppingCart);
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to Add Item into the cart -ShoppingCartController");
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Add Item into the cart -ShoppingCartController");
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "Product ID or CartId is Null");
            }
        }




        [HttpGet]
        [Route("GetCartItem/{UserId}")]
        public async Task<ActionResult<Core.Models.ShoppingCart>> GetCartItem(int UserId)
        {
            try
            {
                Core.Models.ShoppingCart ShoppingCarts = new Core.Models.ShoppingCart();
                ShoppingCarts = await _shoppingCartservice.GetCartItemAsync(UserId);
                if (ShoppingCarts == null)
                {
                    return new Core.Models.ShoppingCart();
                }
                else
                {
                    return ShoppingCarts;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to Get cart item Details - ShoppingCartController");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Get cart item Details - ShoppingCartController");
            }

        }

        [HttpPost]
        [Route("UpdateCount")]
        public async Task<ActionResult<ShoppingCartItem?>> UpdateCount(ShoppingCartItem CartItem)
        {
            try
            {
                ShoppingCartItem UpdatedShoppingCart = new ShoppingCartItem();

                UpdatedShoppingCart = await _shoppingCartservice.UpdateQuntity(CartItem);
                return UpdatedShoppingCart;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to Update cart item Details - ShoppingCartController");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Update cart item Details- ShoppingCartController");
            }

        }

        [HttpPost]
        [Route("RemoveProductFromCart")]
        public async Task<ActionResult<ShoppingCartItem?>> RemoveProductFromCart(ShoppingCartItem RemoveCartEntity)
        {
            try
            {
                ShoppingCartItem RemoveProductFromCart = new ShoppingCartItem();
                RemoveProductFromCart = await _shoppingCartservice.RemoveProductFromCart(RemoveCartEntity);
                return RemoveProductFromCart;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to Remove cart item Details - ShoppingCartController");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Remove cart item Details- ShoppingCartController");
            }

        }


    }
}
