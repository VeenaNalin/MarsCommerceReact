//using MarsCommerce.Core.Interfaces;
//using MarsCommerce.Core.Models;
//using MarsCommerce.ShoppingCart.Service.Services;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MarsCommerce.ShoppingCart.Service.UnitTests
//{
//    public class ShoppingCartServiceTest
//    {
//        private readonly Mock<IRepository<Core.Models.ShoppingCart>> _shoppingCartRepositoryMock;
//        private readonly Mock<IRepository<ShoppingCartItem>> _shoppingCartItemRepositoryMock;
//        private readonly Mock<IRepository<Product>> _productRepositoryMock;
//        private readonly ShoppingCartService _shoppingCartServiceMock;

//        public ShoppingCartServiceTest() 
//        {
//            _shoppingCartRepositoryMock=new Mock<IRepository<Core.Models.ShoppingCart>>();
//            _shoppingCartItemRepositoryMock = new Mock<IRepository<ShoppingCartItem>>();
//            _productRepositoryMock = new Mock<IRepository<Product>>();
//            _shoppingCartServiceMock=new ShoppingCartService(_shoppingCartItemRepositoryMock.Object,
//                                                             _shoppingCartRepositoryMock.Object,
//                                                               _productRepositoryMock.Object);

//        }


//        [Fact]
//        public void AddItemToCart_Success()
//        {
//            //Arrange
//            var shoppingCartItem = new ShoppingCartItem()
//            {
//                CartId = 1,
//                ProductID = 25,
//                Quntity = 2,
//                UnitPrice = 100,
//                TotalPrice = 200,
//            };

//            //Act
//            _shoppingCartItemRepositoryMock.Setup(s=>s.AddAsync(shoppingCartItem))
//                                           .Returns(Task.Run(()=>shoppingCartItem));
//            var result=_shoppingCartServiceMock.AddItemToCart(shoppingCartItem);


//            //Assert
//            var response=Assert.IsType<ShoppingCartItem>(result.Result);


//        }
//    }
//}
