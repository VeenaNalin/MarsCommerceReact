using Ardalis.ApiClient;
using FluentValidation;
using MarsCommerce.Core.Models;
using MarsCommerce.Web.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;


namespace MarsCommerce.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [RequiredScope(scopeRequiredByAPI)]
    public class CatalogController : MarsCommerceBaseController
    {
        const string scopeRequiredByAPI = "demo.read";
        private readonly IConfiguration _configuration;
        private readonly ILogger<CatalogController> _logger;
        private readonly IValidator<ProductVM> _validatorProductVM;

        public CatalogController(HttpService httpService, ILogger<CatalogController> logger, IConfiguration configuration, IValidator<ProductVM> validatorProductVM) : base( httpService, configuration.GetSection("CatalogServiceSettings:BaseUrl").Value?? string.Empty)
        {
            _configuration = configuration;
            _logger = logger;
            _validatorProductVM = validatorProductVM;
        }
        
        [HttpGet("product/{id}")]
        public async Task<ActionResult<ProductVM?>> Get(int id)
        {
            try
            {
                var response = await _httpService.HttpGetAsync<ProductVM>(Constants.Product_Uri+Convert.ToString(id));
                if(response != null)
                {
                    if (response.Code == System.Net.HttpStatusCode.OK) 
                        return Ok(response.Data);
                    
                    else if (response.Code == System.Net.HttpStatusCode.InternalServerError)
                        return StatusCode(StatusCodes.Status500InternalServerError, "Catalog API Returned an error!");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Web api call to get product details failed", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Get Product Details - BFF");
            }
            return NotFound();
        }
        [Route("product/GetAllProduct")]
        public async Task<ActionResult<ProductVM?>> GetAllProducts([FromQuery] int pageIndex, int pageSize, string orderBy)
        {
            try
            {
                
                var response = await _httpService.HttpGetAsync<List<ProductVM>>( "catalog/product/GetAllProduct?pageIndex=" + pageIndex+"&pageSize="+pageSize+"&orderBy="+orderBy);
                if (response != null)
                {
                    if (response.Code == System.Net.HttpStatusCode.OK)
                        return Ok(response.Data);
                    else if (response.Code == System.Net.HttpStatusCode.InternalServerError)
                        return StatusCode(StatusCodes.Status500InternalServerError, "Catalog get all API Returned an error!");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Web api call to get all product  failed", pageIndex );
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Get Product Details - BFF");
            }
            return NotFound();
        }

        [Authorize]
        [HttpGet("getattributes")]
        public async Task<ActionResult<ProductAttribute?>> GetAttributes()
        {
            try
            {
                var response = await _httpService.HttpGetAsync<List<ProductAttribute>>(Constants.Product_Uri + "getattributes");
                if (response != null)
                {
                    if (response.Code == System.Net.HttpStatusCode.OK)
                        return Ok(response.Data);
                    else if (response.Code == System.Net.HttpStatusCode.InternalServerError)
                        return StatusCode(StatusCodes.Status500InternalServerError, "Catalog API Returned an error!");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Web api call to get attributes details failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Get attributes Details - BFF");
            }
            return NotFound();
        }
        [Authorize]
        [HttpGet("getcategories")]
        public async Task<ActionResult<Category?>> getcategories()
        {
            try
            {
                
                var response = await _httpService.HttpGetAsync<List<Category>>(Constants.Product_Uri + "getcategories");
                if (response != null)
                {
                    if (response.Code == System.Net.HttpStatusCode.OK)
                        return Ok(response.Data);
                    else if (response.Code == System.Net.HttpStatusCode.InternalServerError)
                        return StatusCode(StatusCodes.Status500InternalServerError, "Catalog API Returned an error!");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Web api call to get category details failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Get category details  - BFF");
            }
            return NotFound();
        }
        [Authorize]
        [HttpPost("addproduct")]
        public async Task<ActionResult<ProductVM?>> AddProduct(ProductVM product)
        {
            try
            {
                var modelValidate = _validatorProductVM.Validate(product);
                if (modelValidate.IsValid)
                {
                    var response = await _httpService.HttpPostAsync<Product>(Constants.Product_Uri + "addproduct", product);
                    if (response != null)
                    {
                        if (response.Code == System.Net.HttpStatusCode.OK)
                            return Ok(response.Data);
                        else if (response.Code == System.Net.HttpStatusCode.InternalServerError)
                            return StatusCode(StatusCodes.Status500InternalServerError, "Catalog API Returned an error!");
                        else
                            return StatusCode(StatusCodes.Status400BadRequest, response);
                    }
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
                _logger.LogError(ex, "Web api call to Save Product details failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Save Product details - BFF");
            }
            return NotFound();
        }
        [Authorize]
        [HttpPost("updateproduct")]
        public async Task<ActionResult<Product?>> updateproduct(Product product)
        {
            try
            {
                var response = await _httpService.HttpPostAsync<Product>(Constants.Product_Uri + "updateproduct", product);
                if (response != null)
                {
                    if (response.Code == System.Net.HttpStatusCode.OK)
                        return Ok(response.Data);
                    else if (response.Code == System.Net.HttpStatusCode.InternalServerError)
                        return StatusCode(StatusCodes.Status500InternalServerError, "Catalog API Returned an error!");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Web api call to Update Product details failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Update Product details - BFF");
            }
            return NotFound();
        }
        [Authorize]
        [HttpGet("payment")]
        public async Task<ActionResult<PaymentInfo?>> GetPayment()
        {
            try
            {
                var response = await _httpService.HttpGetAsync<PaymentInfo>(Constants.Payment_Uri);
                if (response != null)
                {
                    if (response.Code == System.Net.HttpStatusCode.OK)
                        return Ok(response.Data);
                    else if (response.Code == System.Net.HttpStatusCode.InternalServerError)
                        return StatusCode(StatusCodes.Status500InternalServerError, "Catalog API Returned an error!");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Web api call to get payment details failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Get payment Details - BFF");
            }
            return NotFound();
        }


        //[HttpGet("GetProductById/{id}")]
        //public async Task<ActionResult<Product?>> GetProductById(int id)
        //{
        //    try
        //    {
        //        var response = await _httpService.HttpGetAsync<Product>(Constants.Product_Uri + Convert.ToString(id));
        //        if (response != null)
        //        {
        //            if (response.Code == System.Net.HttpStatusCode.OK)
        //                return Ok(response.Data);

        //            else if (response.Code == System.Net.HttpStatusCode.InternalServerError)
        //                return StatusCode(StatusCodes.Status500InternalServerError, "Catalog API Returned an error!");
        //        }
        //        return NotFound();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Web api call to get product details failed", id);
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Get Product Details - BFF");
        //    }
        //    return NotFound();
        //}
    }
}
