using MarsCommerce.Core.Models;

namespace MarsCommerce.Catalog.Service.Interfaces
{
    public interface ICatalogService
    {
        //Product
        Task<Product?> GetProductDetailsAsync(int productId);
        Task<List<Product?>> GetProductsAsync(int? categoryId, int? brandId, int? tagId, int? pageIndex, int? pageSize, string orderBy, string? search);

        Task<Product> AddProduct(Product product);
        Task<Product> UpdateProduct(Product product);
        Task DeleteProduct(int productId);
        Task<List<ProductAttribute>> GetAtributesAsync();
        Task<List<Category>> GetCategoriesAsync();

       



    }
}
