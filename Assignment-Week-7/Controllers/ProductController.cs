using Assignment_Week_7.Models.DTOs;
using Assignment_Week_7.Models.Entities;
using Assignment_Week_7.Services.IServices;
using Assignment_Week_7.Workers;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Nest;


namespace Assignment_Week_7.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("SearchProductUsingElastic")]
        public async Task<IActionResult> Search(SearchProductDto searchProductDto, int pageIndex, int pageSize)
        {
            var products = await _productService.Search(searchProductDto, pageIndex, pageSize);

            return Ok(products);
        }

        [HttpGet("GetAllProductsUsingElastic")]
        public async Task<IActionResult> GetElasticAll()
        {
            var products = await _productService.GetAllElastic();
            return Ok(products);
        }
        [HttpGet("GetProductByIdInElastic")]
        public async Task<IActionResult> GetElasticById(int id)
        {
            var product = await _productService.GetElasticById(id);
            return Ok(product);
        }
        [HttpPost("AddSampleProductsInElastic")]
        public async Task<IActionResult> ElasticAdd()
        {
            await _productService.ElasticAddSampleProducts();
            return Ok("Products added successfully!");

        }
        [HttpPost("AddProductInElastic")]
        public async Task<IActionResult> ElasticAddProduct(Product product)
        {
            var result = await _productService.ElasticAddProduct(product);
            return Ok(result.Result);
        }
        [HttpPost("AddProductsInElasticFromCSVFile")]
        public async Task<IActionResult> AddBulkElasticFromFile(IFormFile file)
        {
            try {
                await _productService.AddProductsInElasticFromFile(file);
                return Ok("Products added successfully!");
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("UpdateProductsUsingBackgroundServices")]
        public async Task<IActionResult> UpdateProductsUsingBackgroundServices()
        {
            await _productService.UpdateProductsUsingBackgroundServices();
            return Ok("Products updated successfully!");
        }

        [HttpDelete("DeleteAllProductsUsingElastic")]
        public async Task<IActionResult> DeleteAllElastic()
        {
            await _productService.DeleteAllElastic();
            return Ok("Deleted all!");
        }
        [HttpDelete("DeleteProductByIdInElastic")]
        public async Task<IActionResult> DeleteProductByIdInElastic(int id)
        {
            await _productService.DeleteProductByIdInElastic(id);
            return Ok("Product deleted successfully!");
        }
    }
}
