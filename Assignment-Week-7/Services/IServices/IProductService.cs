using Assignment_Week_7.Models.DTOs;
using Assignment_Week_7.Models.Entities;
using Nest;

namespace Assignment_Week_7.Services.IServices
{
    public interface IProductService
    {
        Task<List<Product>> GetAllElastic();
        Task<Product> GetElasticById(int id);
        Task ElasticAddSampleProducts();
        Task<IndexResponse> ElasticAddProduct(Product product);
        Task AddProductsInElasticFromFile(IFormFile file);
        Task UpdateProductsUsingBackgroundServices();
        Task<List<Product>> Search(SearchProductDto searchProductDto, int pageIndex, int pageSize);
        Task DeleteAllElastic();
        Task DeleteProductByIdInElastic(int id);
    }
}
