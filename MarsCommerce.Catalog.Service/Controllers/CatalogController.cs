using FluentValidation;
using MarsCommerce.Catalog.Service.Interfaces;
using MarsCommerce.Core.Models;
using MarsCommerce.Core.Validators;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json.Nodes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MarsCommerce.Catalog.Service.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class CatalogController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly ILogger<CatalogController> _logger;
        private readonly IValidator<Product> _validatorProduct;

        public CatalogController(ICatalogService catalogService, ILogger<CatalogController> logger, IValidator<Product> validatorProduct)
        {
            _catalogService = catalogService;
            _logger = logger;
            _validatorProduct = validatorProduct;
        }

        [HttpGet]
        [Route("product/{Id}")]
        public async Task<ActionResult<Product?>> GetProduct(int Id)
        {
            try
            {
                Product? product = await _catalogService.GetProductDetailsAsync(Id);
                var attributes = product?.Attributes;
                return product;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to Get Product Details - CatalogController", Id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Get Product Details - CatalogController");
            }
            return NoContent();
        }

        [HttpPost]
        [Route("product/addproduct")]
        public async Task<ActionResult<Product?>> AddProduct(Product product)
        {
            try
            {
                var modelValidate = _validatorProduct.Validate(product);
                if (modelValidate.IsValid)
                {
                    return await _catalogService.AddProduct(product);
                }
                else
                {
                    var response = new
                    {
                        Message = "Bad Request - Model not valid",
                        Errors = modelValidate.Errors
                    };
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to Add a new Product - CatalogController", product);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Adda a new Product - CatalogController");
            }
        }

        [HttpPost]
        [Route("product/updateproduct")]
        public async Task<ActionResult<Product?>> UpdateProduct(Product product)
        {
            try
            {
                return await _catalogService.UpdateProduct(product);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update Product - CatalogController", product);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update Product - CatalogController");
            }
        }

        [HttpGet]
        [Route("product/getattributes")]
        public async Task<ActionResult<List<ProductAttribute?>>> GetAttributes()
        {
            try
            {
                return await _catalogService.GetAtributesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get product attributes - CatalogController");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get product attributes - CatalogController");
            }
        }

        [HttpGet]
        [Route("product/getcategories")]
        public async Task<ActionResult<List<Category?>>> GetCategories()
        {
            try
            {
                List<Category?> categories = new List<Category?>();
                return categories = _catalogService.GetCategoriesAsync().Result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get product categories - CatalogController");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get product categories - CatalogController");
            }
        }
        
        

        [HttpGet]
        [Route("product/GetAllProduct")]
        public async Task<ActionResult<List<Product?>>> GetAllProducts(int pageIndex, int pageSize, string orderBy)
        {
            try
            {
                List<Product> product = await _catalogService.GetProductsAsync(null, null, null, pageIndex, pageSize, orderBy, null);
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to Get Product Details - CatalogController");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Get Product Details - CatalogController");
            }

        }
    }
}



