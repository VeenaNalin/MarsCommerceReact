using FluentValidation;
using MarsCommerce.Catalog.Service.Controllers;
using MarsCommerce.Catalog.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace MarsCommerce.Catalog.Service.UnitTests
{
    public class CatalogControllerTest
    {
        private readonly Mock<ICatalogService> _catalogService;
        private readonly Mock<ILogger<CatalogController>> _logger;
        private readonly Mock<IValidator<Product>> _validatorProduct;

        private readonly CatalogController catalogController;

        public CatalogControllerTest()
        {
            _catalogService = new Mock<ICatalogService>();
            _logger = new Mock<ILogger<CatalogController>>();
            _validatorProduct = new Mock<IValidator<Product>>();
            catalogController = new CatalogController(_catalogService.Object, _logger.Object, _validatorProduct.Object);
        }
        [Fact]
        public void Get_ProductDetails_Success()
        {
            // Arrange            
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Description = "Test Description",
                Price = 100,
                ImageUrl = "test_product.jpg",
                StockCount = 1,
                StockKeepingUnit = "S5762567",
                Rating = 4,
                Attributes = new List<ProductAttributeMapping>
                {
                    new ProductAttributeMapping{ Id = 1, ProductId = 1, ProductAttributeId = 1, ProductAttributeValue = "Test Value"},
                    new ProductAttributeMapping{ Id = 2, ProductId = 1, ProductAttributeId = 2, ProductAttributeValue = "Test Value"},
                }

            };
            _catalogService.Setup(x => x.GetProductDetailsAsync(It.IsAny<int>())).ReturnsAsync(product);

            // Act
            var result = catalogController.GetProduct(1).Result;
            // Assert
            _catalogService.Verify(x => x.GetProductDetailsAsync(It.IsAny<int>()), Times.AtLeastOnce);
            var response1 = Assert.IsType<Product>(result.Value);
            Assert.Equal(product.Id, response1.Id);
            Assert.Equal(product.Name, response1?.Name);
            Assert.Equal(product.Description, response1?.Description);
            Assert.Equal(product.Price, response1?.Price);
            Assert.Equal(product.ImageUrl, response1?.ImageUrl);
            Assert.Equal(product.StockCount, response1?.StockCount);
            Assert.Equal(product.Attributes.Count, response1?.Attributes?.Count);
        }
        /// <summary>
        /// GetAll the Product with successs
        /// </summary>
        [Fact]
        public void GetAllProductDetails_Success()
        {
            // Arrange
            int pageIndex = 0;
            int pageSize = 6;
            string orderBy = "Name";
            List<Product?> lstProduct = new List<Product?>();
            Product product = new Product
            {
                Name = "Test",
                Description = "TestProduct",
                ImageUrl = "TestUrl",
                StockKeepingUnit = "10"
            };
            lstProduct.Add(product);

            _catalogService.Setup(x => x.GetProductsAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<string?>())).Returns(Task.Run(() => lstProduct));

            //Act
            var response = catalogController.GetAllProducts(pageIndex, pageSize, orderBy).Result;

            //Assert
            _catalogService.Verify(ar => ar.GetProductsAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<string?>()), Times.Once);
            var response1 = Assert.IsType<List<Product>>(response.Value);
            Assert.Equal(response1[0].Name, "Test");
        }
        /// <summary>
        /// Gets the Product not found.
        /// </summary>
        [Fact]
        public void GetAllProductDetails_NoDataFound()
        {
            // Arrange
            int pageIndex = 0;
            int pageSize = 6;
            string orderBy = "Name";
            List<Product> lstProduct = new List<Product>();


            _catalogService.Setup(x => x.GetProductsAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<string?>())).Returns(Task.Run(() => lstProduct));

            //Act
            var response = catalogController.GetAllProducts(pageIndex, pageSize, orderBy).Result;

            //Assert
            _catalogService.Verify(ar => ar.GetProductsAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<string?>()), Times.AtLeastOnce);
            var response1 = Assert.IsType<List<Product>>(response.Value);
            Assert.Equal(response1.Count, 0);
        }

        /// <summary>
        /// Gets all the categories with success.
        /// </summary>
        [Fact]
        public void Get_Categories_Success()
        {
            // Arrange            
            var category = new List<Category>
            {
                new Category{ Id = 1,Name="Category1"},
                new Category{ Id = 2,Name="Category2"},
                new Category{ Id = 3,Name="Category3"},
                new Category{ Id = 4,Name="Category4"},
                new Category{ Id = 5,Name="Category5"}
            };
            _catalogService.Setup(x => x.GetCategoriesAsync()).Returns(Task.Run(() => category));

            // Act
            var result = catalogController.GetCategories().Result;

            // Assert
            var response1 = Assert.IsType<List<Category>>(result.Value);
            for (int i = 0; i < 5; i++)
            {
                Assert.Equal(category[i].Id, response1[i].Id);
                Assert.Equal(category[i].Name, response1[i].Name);
            }
        }

        /// <summary>
        /// Gets all the product attribute with success.
        /// </summary>
        [Fact]
        public void Get_ProductAttributes_Success()
        {
            // Arrange            
            var productAttributes = new List<ProductAttribute>
            {
                new ProductAttribute{ Id = 1,Name="ProductAttribute1"},
                new ProductAttribute{ Id = 2,Name="ProductAttribute2"},
                new ProductAttribute{ Id = 3,Name="ProductAttribute3"},
                new ProductAttribute{ Id = 4,Name="ProductAttribute4"},
                new ProductAttribute{ Id = 5,Name="ProductAttribute5"}
            };
            _catalogService.Setup(x => x.GetAtributesAsync()).Returns(Task.Run(() => productAttributes));

            // Act
            var result = catalogController.GetAttributes().Result;

            // Assert
            var response1 = Assert.IsType<List<ProductAttribute>>(result.Value);
            for (int i = 0; i < 5; i++)
            {
                Assert.Equal(productAttributes[i].Id, response1[i].Id);
                Assert.Equal(productAttributes[i].Name, response1[i].Name);
            }
        }

        /// <summary>
        /// Add product with success.
        /// </summary>
        [Fact]
        public void Add_Product_Success()
        {
            // Arrange
            var product = new Product
            {
                Description = "Product1 description",
                ImageUrl = "Product1.jpg",
                Name = "Product1",
                CategoryId = 1,
                StockKeepingUnit = "100",
                Price = 100,
                Rating = null,
                StockCount = 50,
                Attributes = new List<ProductAttributeMapping>
               {
                   new ProductAttributeMapping { ProductAttributeId=1, ProductAttributeValue= "Sample Value"},
                   new ProductAttributeMapping { ProductAttributeId=2, ProductAttributeValue= "Sample Value2"},
               }
            };
            _catalogService.Setup(x => x.AddProduct(product).Result).Returns(product);

            // Act
            var result = catalogController.AddProduct(product).Result;

            // Assert
            var response = Assert.IsType<Product>(result.Value);
            Assert.Equal(product.Name, response.Name);
            Assert.Equal(product.Description, response.Description);
            Assert.Equal(product.CategoryId, response.CategoryId);
        }

        /// <summary>
        /// Add product with exception.
        /// </summary>
        [Fact]
        public void Add_Product_NotSuccess()
        {
            // Arrange            
            var product = new Product
            {
                Description = null,
                ImageUrl = "Product1.jpg",
                Name = null,
                CategoryId = 1,
                StockKeepingUnit = "100",
                Price = 100,
                Rating = null,
                StockCount = 50,
                Attributes = new List<ProductAttributeMapping>
               {
                   new ProductAttributeMapping { ProductAttributeId=1, ProductAttributeValue= "Sample Value"},
                   new ProductAttributeMapping { ProductAttributeId=2, ProductAttributeValue= "Sample Value2"},
               }
            };
            // Act
            _catalogService.Setup(x => x.AddProduct(product).Result).Throws<HttpRequestException>();
            var objectResult = catalogController.AddProduct(product).Result.Result;
            // Assert
            var res = Assert.IsType<ObjectResult>(objectResult);
            Assert.Equal(500, ((ObjectResult)res).StatusCode);
            Assert.Equal("Failed to Adda a new Product - CatalogController", ((ObjectResult)res).Value);

        }
        
        /// <summary>
        /// Update product with success.
        /// </summary>
        [Fact]
        public void Update_Product_Success()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Description = "Product1 description",
                ImageUrl = "Product1.jpg",
                Name = "Product",
                CategoryId = 1,
                StockKeepingUnit = "100",
                Price = 100,
                Rating = null,
                StockCount = 50,
                Attributes = new List<ProductAttributeMapping>
               {
                   new ProductAttributeMapping { ProductAttributeId=1, ProductAttributeValue= "Sample Value"},
                   new ProductAttributeMapping { ProductAttributeId=2, ProductAttributeValue= "Sample Value2"},
               }
            };

            var productUpdated = new Product
            {
                Id = 1,
                Description = "Product1 description",
                ImageUrl = "Product1.jpg",
                Name = "Product new name",
                CategoryId = 1,
                StockKeepingUnit = "100",
                Price = 100,
                Rating = null,
                StockCount = 50,
                Attributes = new List<ProductAttributeMapping>
               {
                   new ProductAttributeMapping { ProductAttributeId=1, ProductAttributeValue= "Sample Value"},
                   new ProductAttributeMapping { ProductAttributeId=2, ProductAttributeValue= "Sample Value2"},
               }
            };
            _catalogService.Setup(x => x.UpdateProduct(product).Result).Returns(productUpdated);

            // Act
            var result = catalogController.UpdateProduct(product).Result;

            // Assert
            var response = Assert.IsType<Product>(result.Value);
            Assert.NotEqual(product.Name, response.Name);
            Assert.Equal(product.Description, response.Description);
            Assert.Equal(product.Id, response.Id);
        }

        /// <summary>
        /// Update product with exception.
        /// </summary>
        [Fact]
        public void Update_Product_NotSuccess()
        {
            // Arrange            
            var product = new Product
            {
                Id=1,
                Description = null,
                ImageUrl = "Product1.jpg",
                Name = null,
                CategoryId = 1,
                StockKeepingUnit = "100",
                Price = 100,
                Rating = null,
                StockCount = 50,
                Attributes = new List<ProductAttributeMapping>
               {
                   new ProductAttributeMapping { ProductAttributeId=1, ProductAttributeValue= "Sample Value"},
                   new ProductAttributeMapping { ProductAttributeId=2, ProductAttributeValue= "Sample Value2"},
               }
            };
            // Act
            _catalogService.Setup(x => x.UpdateProduct(product).Result).Throws<HttpRequestException>();
            var objectResult = catalogController.UpdateProduct(product).Result.Result;
            // Assert
            var res = Assert.IsType<ObjectResult>(objectResult);
            Assert.Equal(500, ((ObjectResult)res).StatusCode);
            Assert.Equal("Failed to update Product - CatalogController", ((ObjectResult)res).Value);

        }
    }
}
