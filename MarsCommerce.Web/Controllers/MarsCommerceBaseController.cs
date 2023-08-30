using Ardalis.ApiClient;
using Microsoft.AspNetCore.Mvc;

namespace MarsCommerce.Web.Controllers
{
    public class MarsCommerceBaseController : Controller
    {
        protected readonly HttpService _httpService;
        public MarsCommerceBaseController(HttpService httpService, string baseUrl)
        {
            if(baseUrl == string.Empty)
            {
                throw new ArgumentNullException("Invalid Backend Web Api Base Url");
            }
            _httpService = httpService;
            _httpService.SetAuthorization(Guid.NewGuid().ToString());
            _httpService.SetBaseUri("*/*", new Uri(baseUrl));
        }
    }
}
