using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SampleForTahaBazar.Entities.Models;
using SampleForTahaBazar.Services;

namespace SampleForTahaBazar.WebApplication.Pages.Products
{
    [ApiController]
    [Route("api/products")]
    public class ProductsApiController : ControllerBase
    {
        private readonly IProductsService _productsService;

        public ProductsApiController(IProductsService productsService)
        {
            _productsService = productsService;
        }
        [HttpGet()]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            return Ok(_productsService.GetProducts());
        }
        [HttpPost()]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            await _productsService.AddProduct(product);
            return Ok();
        }
    }
}