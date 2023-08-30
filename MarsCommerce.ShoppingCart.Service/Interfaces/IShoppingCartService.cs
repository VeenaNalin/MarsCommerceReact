using MarsCommerce.Core.Models;
using System.Linq.Expressions;

namespace MarsCommerce.ShoppingCart.Service.Interfaces
{
    public interface IShoppingCartService
    {
        Task<Product> GetProductDetailsById(int productId);
        Task<Core.Models.ShoppingCart> AddItemToCart(Core.Models.ShoppingCart cart);
        Task<Core.Models.ShoppingCart> GetCartItemAsync(int CartId);
        Task<ShoppingCartItem> UpdateQuntity(ShoppingCartItem CartItem);
        Task<ShoppingCartItem> RemoveProductFromCart(ShoppingCartItem cartEntity);
       
        //Task<ShoppingCartItem> AddNewCartItem(ShoppingCartItem item);
        Task<List<Core.Models.ShoppingCart>> GetAllCartItemsAsync(Expression<Func<Core.Models.ShoppingCart, bool>> filter);
    }
}
