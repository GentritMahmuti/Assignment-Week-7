using Assignment_Week_7.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;


namespace Assignment_Week_7.Controllers
{
    public class ProductController : Controller
    {
        private readonly ElasticClient _elasticClient;
        public ProductController(ElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }
        [HttpPost("elastic")]
        public async Task<IActionResult> AddElastic()
        {
            var product = new Product
            {
                ProductName = "Product 1",
                Description = "Dscription of product 1",
                Price = 10.99,
                Category = "Category A"
            };
            var result = await _elasticClient.IndexAsync<Product>(product, x => x.Index("products_index").Id(product.ProductName));
            return Ok(result.Result);

        }
        [HttpGet("get-elastic-by-id")]
        public async Task<IActionResult> GetElastic(string product, string index)
        {
            var response = await _elasticClient.GetAsync<Product>(product, x => x.Index(index));
            return Ok(response.Source);
        }


    }
}
