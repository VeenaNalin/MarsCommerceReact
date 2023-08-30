using MarsCommerce.Core.Interfaces;
using MarsCommerce.Core.Models;
using MarsCommerce.ShoppingCart.Service.Interfaces;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Linq.Expressions;

namespace MarsCommerce.ShoppingCart.Service.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<Core.Models.ShoppingCart> _ShoppingCartRepository;
        private readonly IRepository<ShoppingCartItem> _ShoppingCartItemRepository;       
        private readonly IRepository<Product> _ProductRepository;

        public int Quntity { get; private set; }

        public ShoppingCartService(IRepository<Core.Models.ShoppingCart> shoppingCartEntityRepository, IRepository<ShoppingCartItem> ShoppingCartRepository,                                   
                                   IRepository<Product> ProductRepository)
        {
            _ShoppingCartRepository = shoppingCartEntityRepository; 
            _ShoppingCartItemRepository = ShoppingCartRepository;           
            _ProductRepository = ProductRepository;
        }


        public async Task<Product> GetProductDetailsById(int productId)
        {
            return await _ProductRepository.GetAsync(productId);
        }


        public async Task<Core.Models.ShoppingCart> AddItemToCart(Core.Models.ShoppingCart? cart)
        {
            Core.Models.ShoppingCart shopCart = new Core.Models.ShoppingCart();
            //Get CartItems based on userid
            shopCart = GetCartItemAsync(cart.UserId).Result;
            if (shopCart==null)
            {
                List<ShoppingCartItem> item = new List<ShoppingCartItem>();
               
                foreach (var cartItems in cart.Items)
                {
                    Product product = await GetProductDetailsById((int)cartItems.ProductID);                    
                    cartItems.ProductID = product.Id;
                    cartItems.UnitPrice = product.Price;
                    cartItems.Quntity = 1;
                    cartItems.TotalPrice = product.Price;
                    
                }                            
                  await _ShoppingCartRepository.AddAsync(cart);               
            }
            else
            {
                Core.Models.ShoppingCart shopCarts = GetCartItemAsync(cart.UserId).Result;
                foreach (var cartItem in cart.Items)
                {
                    var product_exist_id = 0;
                    bool already_exist_product_ind = false;
                    foreach(var productItem in shopCarts.Items)
                    {
                        if(productItem.ProductID== cartItem.ProductID)
                        {
                            product_exist_id = productItem.Quntity;
                            already_exist_product_ind = true;
                            cartItem.Id = productItem.Id;
                        }
                        
                    }
                    
                    Product product = await GetProductDetailsById((int)cart.Items[0].ProductID);
                    cartItem.UnitPrice = product.Price;
                    cartItem.Quntity = product_exist_id +1;
                    cartItem.TotalPrice = cartItem.Quntity * product.Price;
                    cartItem.CartId = shopCarts.Id;
                    cart.Id = shopCarts.Id;
                    if (already_exist_product_ind==false)
                    {
                        await _ShoppingCartItemRepository.AddAsync(cartItem);
                    }
                    else
                    {

                        UpdateQuntity(cartItem);
                    }
                    
                }
               

                }
            return cart;                     

        }
        

        public async Task<List<Core.Models.ShoppingCart>> GetAllCartItemsAsync(Expression<Func<Core.Models.ShoppingCart, bool>> filter)
        {
            return await _ShoppingCartRepository.GetAllByAsync(filter);
        }

        public async Task<Core.Models.ShoppingCart?> GetCartItemAsync(int UserId)
        {
            int count = GetAllCartItemsAsync(p => p.UserId.Equals(UserId)).Result.Count();
            int shoppingCartId = 0;
            if (count>0)
            {
                shoppingCartId = GetAllCartItemsAsync(p => p.UserId.Equals(UserId)).Result.FirstOrDefault().Id;           
            }
            return await _ShoppingCartRepository.GetAsync(shoppingCartId);


        }

        public async Task<ShoppingCartItem> UpdateQuntity(ShoppingCartItem CartItem)
        {

            Core.Models.ShoppingCart? Carts = await _ShoppingCartRepository.GetAsync(CartItem.CartId);
            List<ShoppingCartItem>? list = Carts.Items;
            ShoppingCartItem Item = list.Where(s => s.ProductID == CartItem.ProductID).First();
            Item.Quntity = CartItem.Quntity;
            Item.TotalPrice = CartItem.Quntity * CartItem.UnitPrice;
            CartItem = await _ShoppingCartItemRepository.UpdateAsync(Item);
            return CartItem;
        }

        public async Task<ShoppingCartItem> RemoveProductFromCart(ShoppingCartItem RemoveCart)
        {
            Core.Models.ShoppingCart? RemoveItem = await _ShoppingCartRepository.GetAsync(RemoveCart.CartId);
            List<ShoppingCartItem>? list = RemoveItem.Items;
            ShoppingCartItem Item = list.Where(s => s.ProductID == RemoveCart.ProductID).First();
            RemoveCart = await _ShoppingCartItemRepository.DeleteAsync(Item);

            list = _ShoppingCartItemRepository.GetAllByAsync(x=>x.CartId == RemoveCart.CartId).Result;
            if (list.Count == 0)
            {
                await _ShoppingCartRepository.DeleteAsync(RemoveItem);
            }
            //UpdateQuntity(Item);
            return RemoveCart;
        }



    }
}
