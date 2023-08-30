//using MarsCommerce.Core.Models;
//using MarsCommerce.ShoppingCart.Service.Controllers;
//using MarsCommerce.ShoppingCart.Service.Interfaces;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using Moq;

//namespace MarsCommerce.ShoppingCart.Service.UnitTest
//{
//    public class ShoppingCartControllerTest
//    {

//        private readonly Mock<IShoppingCartService> _shoppingCartService;
//        private readonly Mock<ILogger<ShoppingCartController>> _shoppingCartController;
//        private readonly ShoppingCartController shoppingCartController;
        

//        public ShoppingCartControllerTest()
//        {
//            _shoppingCartService=new Mock<IShoppingCartService>();
//            _shoppingCartController=new Mock<ILogger<ShoppingCartController>>();
//            shoppingCartController = new ShoppingCartController(_shoppingCartService.Object,
//                                                                _shoppingCartController.Object);
//        }

//        [Fact]
//        public void AddToCartItem_Success()
//        {
//            //Arrange
//            var shoppingCartItem = new ShoppingCartItem()
//            {
//                CartId = 1,
//                ProductID=25,
//                Quntity=2,
//                UnitPrice=100,
//                TotalPrice=200,
//            };     
//            _shoppingCartService.Setup(s=>s.AddItemToCart(shoppingCartItem).Result).Returns(shoppingCartItem);

//            //Act
//            var result=shoppingCartController.AddToCartItem(shoppingCartItem).Result;

//            //Assert
//            var response=Assert.IsType<ShoppingCartItem>(result.Value);
//            Assert.Equal(shoppingCartItem.CartId,response.CartId);
//            Assert.Equal(shoppingCartItem.ProductID, response.ProductID);
//            Assert.Equal(shoppingCartItem.Quntity, response.Quntity);
//            Assert.Equal(shoppingCartItem.TotalPrice, response.TotalPrice);
//        }


//        [Fact]
//        public void AddToCartItem_Fail()
//        {
//            //Arrange
//            var shopCartItem = new ShoppingCartItem()
//            {
//                CartId = null,
//                ProductID = null,
//                Quntity = 2,
//                UnitPrice = 100,
//                TotalPrice = 200,
//            };

//            //Act
//            _shoppingCartService.Setup(s => s.AddItemToCart(shopCartItem).Result).Throws<HttpRequestException>();
//            var scResult = shoppingCartController.AddToCartItem(shopCartItem).Result.Result;

//            //Assert
//            var result = Assert.IsType<ObjectResult>(scResult);
//            Assert.Equal(500,((ObjectResult)result).StatusCode);
//            Assert.Equal("Failed to Add Item into the cart -ShoppingCartController",((ObjectResult)result).Value);
//        }
//    }
//}