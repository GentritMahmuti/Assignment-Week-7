using Assignment_Week_7.Models.DTOs;
using Assignment_Week_7.Models.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Nest;


namespace Assignment_Week_7.Controllers
{
    public class ProductController : Controller
    {
        private readonly IValidator<SearchProductDto> _searchProductDtoValidator;
        private readonly ElasticClient _elasticClient;
        
        public ProductController(ElasticClient elasticClient, IValidator<SearchProductDto> searchProductDtoValidator)
        {
            _elasticClient = elasticClient;
            _searchProductDtoValidator = searchProductDtoValidator;
        }

        [HttpGet("getAllProductsUsingElastic")]
        public async Task<IActionResult> GetElasticAll()
        {
            var response = await _elasticClient.SearchAsync<Product>(s =>
            s.Index("products")
               .Query(q => q
                  .MatchAll())
               );
            return Ok(response.Documents);
        }
        [HttpGet("getProductByIdInElastic")]
        public async Task<IActionResult> GetElasticById(int id)
        {
            var response = await _elasticClient.GetAsync<Product>(id, x => x.Index("products"));
            return Ok(response.Source);
        }
        [HttpPost("AddProductsInElastic")]
        public async Task<IActionResult> ElasticAdd()
        {
            var products = new List<Product>
            {
                new Product(){
                Id = 1,
                ProductName = "Product 1",
                Description = "Description of product 1",
                Price = 10.99,
                Category = "Category A" },
                new Product(){
                Id = 2,
                ProductName = "Product 2",
                Description = "Description of product 2",
                Price = 20.99,
                Category = "Category B" },
                new Product(){
                Id = 3,
                ProductName = "Product 3",
                Description = "Description sale of product 3",
                Price = 30.99,
                Category = "Category A" },
                new Product(){
                Id = 4,
                ProductName = "Product 4",
                Description = "Description of product 4",
                Price = 40.99,
                Category = "Category C" },
                new Product(){
                Id = 5,
                ProductName = "Product 5",
                Description = "Description of product 5",
                Price = 50.99,
                Category = "Category B" },
                new Product(){
                Id = 6,
                ProductName = "Product 6",
                Description = "Description of product 6",
                Price = 60.99,
                Category = "Category C" },
                new Product(){
                Id = 7,
                ProductName = "Product 7",
                Description = "Description discount of product 7",
                Price = 70.99,
                Category = "Category A" },
                new Product(){
                Id = 8,
                ProductName = "Product 8",
                Description = "Description of product 8",
                Price = 80.99,
                Category = "Category B" },
                new Product(){
                Id = 9,
                ProductName = "Product 9",
                Description = "Description discount sale of product 9",
                Price = 90.99,
                Category = "Category C" },
                new Product(){
                Id = 10,
                ProductName = "Product 10",
                Description = "Description of product 10",
                Price = 100.99,
                Category = "Category A" },

            };


            var response = _elasticClient.Bulk(x => x.Index("products").IndexMany(products));
            return Ok(response);

        }
        [HttpPost("addProductsInElasticFromCSVFile")]
        public async Task<IActionResult> AddBulkElasticFromFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new Exception("The file given is null or empty!");
            }

            var csvRecords = new List<string>();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                {
                    csvRecords.Add(reader.ReadLine().Replace("\"", ""));
                }
            }

            List<Product> productsToCreate = new();
            csvRecords.RemoveAt(0);
            var id = 1;
            foreach (var line in csvRecords)
            {
                var csvLine = line.Split(",");
                Product product = new Product
                {
                    Id = id++,
                    ProductName = csvLine[0],
                    Description = csvLine[1],
                    Price = double.Parse(csvLine[2]),
                    Category = csvLine[3],
                };
                productsToCreate.Add(product);
            }

            var result = await _elasticClient.BulkAsync(x =>
                x.Index("products").IndexMany(productsToCreate));

            return Ok(result.IsValid);
        }
        [HttpGet("SearchProductUsingElastic")]
        public async Task<IActionResult> Search(SearchProductDto searchProductDto,int pageIndex, int pageSize)
        {
            await _searchProductDtoValidator.ValidateAndThrowAsync(searchProductDto);

            var result = await _elasticClient.SearchAsync<Product>(s => s
                .Index("products")
                .From((pageIndex-1)*pageSize)
                .Size(pageSize)
                .Query(q => q
                    .MatchPhrase(m => m
                        .Field(f => f.ProductName)
                            .Query(searchProductDto.ProductName)
                ) && q
                .MatchPhrase(m => m
                    .Field(f => f.Category)
                        .Query(searchProductDto.Category)
                ) && q
                .Range(r => r
                    .Field(f => f.Price)
                        .GreaterThanOrEquals(searchProductDto.Price)
                ) || q
                .MatchPhrase(m => m
                    .Field(f => f.Description)
                        .Query("discount")
                        .Boost(3)
                    ) || q
                    .MatchPhrase(m => m
                    .Field(f => f.Description)
                        .Query("sale")
                        .Boost(3)
                         )
                    )
                .Sort(s=>s
                .Descending(SortSpecialField.Score)
                     )
                ); 

            return Ok(result.Documents);
        }
        [HttpDelete("DeleteAllProductsUsingElastic")]
        public async Task<IActionResult> DeleteAllElastic()
        {
            var deleteResponse = _elasticClient.DeleteByQuery<Product>(del => del
                   .Index("products")
                   .Query(q => q.MatchAll())
               );
            return Ok("Deleted all!");
        }
        [HttpDelete("DeleteProductByIdInElastic")]
        public async Task<IActionResult> DeleteProductByIdInElastic(int id)
        {
            var deleteResponse = _elasticClient.Delete<Product>(id, d => d
            .Index("products")
            );
            if(!deleteResponse.IsValid)
            {
                throw new Exception("There isn't a product with that ID");
            }
            return Ok("Product deleted successfully!");
        }

    }
}
