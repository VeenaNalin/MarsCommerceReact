using Moq;
using System.Linq.Expressions;

namespace MarsCommerce.Catalog.Service.UnitTests
{
    public class CatalogServiceTest
    {
        private readonly Mock<IRepository<Product>> _productRepositoryMock;
        private readonly Mock<IRepository<ProductAttribute>> _attributeRepositoryMock;
        private readonly Mock<IRepository<Category>> _categoryRepositoryMock;
        private readonly CatalogService catalogService;
        public CatalogServiceTest()
        {
            _productRepositoryMock = new Mock<IRepository<Product>>();           
            _categoryRepositoryMock = new Mock<IRepository<Category>>();
            _attributeRepositoryMock = new Mock<IRepository<ProductAttribute>>();
            catalogService = new CatalogService(_productRepositoryMock.Object, _attributeRepositoryMock.Object, _categoryRepositoryMock.Object);
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
            _productRepositoryMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync(product);

            // Act
            var result = catalogService.GetProductDetailsAsync(1).Result;
            // Assert
            Assert.Equal(product.Id, result?.Id);
            Assert.Equal(product.Name, result?.Name);
            Assert.Equal(product.Description, result?.Description);
            Assert.Equal(product.Price, result?.Price);
            Assert.Equal(product.ImageUrl, result?.ImageUrl);
            Assert.Equal(product.StockCount, result?.StockCount);
            Assert.Equal(product.Attributes.Count, result?.Attributes?.Count);
        }
        /// <summary>
        /// Gets the Product with successs
        /// </summary>
        [Fact]
        public void GetAllProductDetails_Success()
        {
            // Arrange
            int categoryId = 0;
            int brandId = 0;
            int tagId = 0;
            int pageIndex = 0;
            int pageSize = 6;
            string orderBy = "Name";
            string search = null;
            List<Product> lstProduct = new List<Product>();
            lstProduct.Add(new Product()
            {
                Name = "Test",
                Description = "TestProduct",
                ImageUrl = "TestUrl",
                StockKeepingUnit = "10"
            });

            _productRepositoryMock.Setup(x => x.GetAllByAsync(It.IsAny<Expression<Func<Product, bool>>>())).Returns(Task.Run(() => lstProduct));

            //Act
            var result = catalogService.GetProductsAsync(categoryId, brandId, tagId, pageIndex, pageSize, orderBy, search);

            //Assert
            _productRepositoryMock.Verify(ar => ar.GetAllByAsync(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once);
            Assert.True(result.Result.Count > 0);
        }
        /// <summary>
        /// Gets the Product not found.
        /// </summary>
        [Fact]
        public void GetAllProductDetails_NoDataFound()
        {
            // Arrange
            int categoryId = 0;
            int brandId = 0;
            int tagId = 0;
            int pageIndex = 0;
            int pageSize = 6;
            string orderBy = "Name";
            string search = null;
            List<Product> lstProduct = new List<Product>();

            _productRepositoryMock.Setup(x => x.GetAllByAsync(It.IsAny<Expression<Func<Product, bool>>>())).Returns(Task.Run(() => lstProduct));

            //Act
            var result = catalogService.GetProductsAsync(categoryId, brandId, tagId, pageIndex, pageSize, orderBy, search);

            //Assert
            _productRepositoryMock.Verify(ar => ar.GetAllByAsync(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once);
            Assert.True(result.Result.Count == 0);
        }


        /// <summary>
        /// Gets the categories with successs
        /// </summary>
        [Fact]
        public void GetAllCategories_Success()
        {
            // Arrange         
            var categories = new List<Category>
            {
                new Category{ Id = 1,Name="Category1"},
                new Category{ Id = 2,Name="Category2"},
                new Category{ Id = 3,Name="Category3"},
                new Category{ Id = 4,Name="Category4"},
                new Category{ Id = 5,Name="Category5"}
            };

            _categoryRepositoryMock.Setup(x => x.GetAllByAsync(It.IsAny<Expression<Func<Category, bool>>>())).Returns(Task.Run(() => categories));

            //Act
            var result = catalogService.GetCategoriesAsync();

            //Assert
            _categoryRepositoryMock.Verify(c => c.GetAllByAsync(It.IsAny<Expression<Func<Category, bool>>>()), Times.Once);
            Assert.True(result.Result.Count > 0);
            var response = Assert.IsType<List<Category>>(result.Result);
            for (int i = 0; i < 5; i++)
            {
                Assert.Equal(categories[i].Id, response[i].Id);
                Assert.Equal(categories[i].Name, response[i].Name);
            }
        }

        /// <summary>
        /// Gets the product attributes with successs
        /// </summary>
        [Fact]
        public void GetAllProductAttributes_Success()
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

            _attributeRepositoryMock.Setup(x => x.GetAllByAsync(It.IsAny<Expression<Func<ProductAttribute, bool>>>())).Returns(Task.Run(() => productAttributes));

            //Act
            var result = catalogService.GetAtributesAsync();

            //Assert
            _attributeRepositoryMock.Verify(c => c.GetAllByAsync(It.IsAny<Expression<Func<ProductAttribute, bool>>>()), Times.Once);
            Assert.True(result.Result.Count > 0);
            var response = Assert.IsType<List<ProductAttribute>>(result.Result);
            for (int i = 0; i < 5; i++)
            {
                Assert.Equal(productAttributes[i].Id, response[i].Id);
                Assert.Equal(productAttributes[i].Name, response[i].Name);
            }
        }

        /// <summary>
        /// Add a new Product with successs
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
                    new ProductAttributeMapping { ProductAttributeId = 1, ProductAttributeValue = "Sample Value" },
                    new ProductAttributeMapping { ProductAttributeId = 2, ProductAttributeValue = "Sample Value2" },
                }
            };

            //Act
            _productRepositoryMock.Setup(x => x.AddAsync(product)).Returns(Task.Run(() => product));
            var result = catalogService.AddProduct(product);

            //Assert
            var response = Assert.IsType<Product>(result.Result);
        }

        /// <summary>
        /// Add a new Product with not successs
        /// </summary>
        [Fact]
        public void Add_Product_NotSuccess()
        {
            // Arrange         
            var product = new Product
            {
                Description = "Product1 description",
                ImageUrl = "Product1.jpg",
                Name = null,
                CategoryId = 1,
                StockKeepingUnit = "100",
                Price = 100,
                Rating = null,
                StockCount = 50,
                Attributes = new List<ProductAttributeMapping>
                {
                    new ProductAttributeMapping { ProductAttributeId = 1, ProductAttributeValue = "Sample Value" },
                    new ProductAttributeMapping { ProductAttributeId = 2, ProductAttributeValue = "Sample Value2" },
                }
            };

            var productResult = new Product
            {
                Id = 0,
                Description = "Product1 description",
                ImageUrl = "Product1.jpg",
                Name = null,
                CategoryId = 1,
                StockKeepingUnit = "100",
                Price = 100,
                Rating = null,
                StockCount = 50,
                Attributes = new List<ProductAttributeMapping>
                {
                    new ProductAttributeMapping { ProductAttributeId = 1, ProductAttributeValue = "Sample Value" },
                    new ProductAttributeMapping { ProductAttributeId = 2, ProductAttributeValue = "Sample Value2" },
                }
            };

            //Act
            _productRepositoryMock.Setup(x => x.AddAsync(product).Result).Returns(productResult);
            var result = catalogService.AddProduct(product);

            // Assert            
            _productRepositoryMock.Verify(ar => ar.AddAsync(product), Times.Once);
            Assert.Equal(0, productResult.Id);

        }
        
        /// <summary>
        /// Update Product with successs
        /// </summary>
        [Fact]
        public void Update_Product_Success()
        {
            // Arrange         
            var product = new Product
            {
                Id=1,
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
                    new ProductAttributeMapping { ProductAttributeId = 1, ProductAttributeValue = "Sample Value" },
                    new ProductAttributeMapping { ProductAttributeId = 2, ProductAttributeValue = "Sample Value2" },
                }
            };

            var productUpdated = new Product
            {
                Id = 1,
                Description = "Product1 description",
                ImageUrl = "Product1.jpg",
                Name = "Product1 new name",
                CategoryId = 1,
                StockKeepingUnit = "100",
                Price = 100,
                Rating = null,
                StockCount = 50,
                Attributes = new List<ProductAttributeMapping>
                {
                    new ProductAttributeMapping { ProductAttributeId = 1, ProductAttributeValue = "Sample Value" },
                    new ProductAttributeMapping { ProductAttributeId = 2, ProductAttributeValue = "Sample Value2" },
                }
            };

            //Act
            _productRepositoryMock.Setup(x => x.UpdateAsync(product)).Returns(Task.Run(() => productUpdated));
            var result = catalogService.UpdateProduct(product);

            //Assert
            var response = Assert.IsType<Product>(result.Result);
            Assert.Equal("Product1 new name", response.Name);
            Assert.NotEqual(product.Name, response.Name);
        }   
        
        /// <summary>
        /// Update Product with not successs
        /// </summary>
        [Fact]
        public void Update_Product_NotSuccess()
        {
            // Arrange         
            var product = new Product
            {
                Id=1,
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
                    new ProductAttributeMapping { ProductAttributeId = 1, ProductAttributeValue = "Sample Value" },
                    new ProductAttributeMapping { ProductAttributeId = 2, ProductAttributeValue = "Sample Value2" },
                }
            };

            var productUpdated = new Product
            {
                Id = 1,
                Description = "Product1 description new description",
                ImageUrl = "Product1.jpg",
                Name = "Product1 new name",
                CategoryId = 1,
                StockKeepingUnit = "100",
                Price = 100,
                Rating = null,
                StockCount = 50,
                Attributes = new List<ProductAttributeMapping>
                {
                    new ProductAttributeMapping { ProductAttributeId = 1, ProductAttributeValue = "Sample Value" },
                    new ProductAttributeMapping { ProductAttributeId = 2, ProductAttributeValue = "Sample Value2" },
                }
            };

            //Act
            _productRepositoryMock.Setup(x => x.UpdateAsync(product)).Returns(Task.Run(() => product));
            var result = catalogService.UpdateProduct(product);

            //Assert
            var response = Assert.IsType<Product>(result.Result);
            Assert.Equal("Product1", response.Name);
            Assert.Equal("Product1 description", response.Description);
            Assert.Equal(product.Name, response.Name);
        }        
    }
}