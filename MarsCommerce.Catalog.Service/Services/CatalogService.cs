using MarsCommerce.Catalog.Service.Interfaces;
using MarsCommerce.Core.Interfaces;
using MarsCommerce.Core.Models;
using System.Reflection;

namespace MarsCommerce.Catalog.Service
{
    public class CatalogService : ICatalogService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductAttribute> _attributeRepository;
        private readonly IRepository<Category> _categoryRepository;
        public CatalogService(IRepository<Product> productRepository, IRepository<ProductAttribute> attributeRepository, IRepository<Category> categoryRepository)
        {
            _productRepository = productRepository;
            _attributeRepository = attributeRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task<Product> AddProduct(Product product)
        {
            return await _productRepository.AddAsync(product);
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            return await _productRepository.UpdateAsync(product);
        }


        public Task DeleteProduct(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<Product?> GetProductDetailsAsync(int productId)
        {
            return await _productRepository.GetAsync(productId);
        }

        public async Task<List<ProductAttribute?>> GetAtributesAsync()
        {            
            return await _attributeRepository.GetAllByAsync(x => x.Id > 0);
        }

        public async Task<List<Category?>> GetCategoriesAsync()
        {
            List<Category?> categories = new List<Category?>();
            return categories = _categoryRepository.GetAllByAsync(x => x.Id > 0).Result;
        }

        public async Task<List<Product>> GetProductsAsync(int? categoryId, int? brandId, int? tagId, int? pageIndex, int? pageSize, string orderBy, string? search)
        {

            var propertyInfo = typeof(Product).GetProperty(orderBy);
            List<Product> product = new List<Product>();
            if (pageSize > 0)
            {
                return product =_productRepository.GetAllByAsync(x => x.Id > 0).Result.OrderBy(e => propertyInfo.GetValue(e, null)).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();
            }
            else
            {
                return product =  _productRepository.GetAllByAsync(x => x.Id > 0).Result.OrderBy(e => propertyInfo.GetValue(e, null)).ToList();
            }
            //await ProductRepositoty.GetAllByAsync(x => x.Id < 500).Result.Skip(pageIndex.Value).Take(pageSize.Value)
            //return Product;
        }

       
    }
}
