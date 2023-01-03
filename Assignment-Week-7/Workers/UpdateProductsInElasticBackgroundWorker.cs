using Assignment_Week_7.Models.Entities;
using Nest;

namespace Assignment_Week_7.Workers
{
    public class UpdateProductsInElasticBackgroundWorker : IHostedService
    {
        
        private Timer _timer;
        private IServiceProvider _serviceProvider;
        public UpdateProductsInElasticBackgroundWorker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdateElasticIndex, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }

        private async void UpdateElasticIndex(object state)
        {
            
            var newProductData = GetNewProductData();
            using var scope = _serviceProvider.CreateScope();
            var elasticService = scope.ServiceProvider.GetRequiredService<ElasticClient>();
            var bulkResponse = await elasticService.BulkAsync(b => b
                .Index("products")
                .IndexMany(newProductData)
            );

            if (!bulkResponse.IsValid)
            {
                Console.Error.WriteLine("Error updating Elasticsearch index: {0}", bulkResponse.DebugInformation);
            }
        }

        private IEnumerable<Product> GetNewProductData()
        {
            var products = new List<Product>
            {
                new Product(){
                Id = 1,
                ProductName = "Product 1 (Updated)",
                Description = "Description of product 1 (Updated)",
                Price = 10.99,
                Category = "Category A" },
                new Product(){
                Id = 2,
                ProductName = "Product 2 (Updated)",
                Description = "Description of product 2 (Updated)",
                Price = 20.99,
                Category = "Category B" },
                new Product(){
                Id = 3,
                ProductName = "Product 3 (Updated)",
                Description = "Description sale of product 3 (Updated)",
                Price = 30.99,
                Category = "Category A" },
                new Product(){
                Id = 4,
                ProductName = "Product 4 (Updated)",
                Description = "Description of product 4 (Updated)",
                Price = 40.99,
                Category = "Category C" },
                new Product(){
                Id = 5,
                ProductName = "Product 5 (Updated)",
                Description = "Description of product 5 (Updated)",
                Price = 50.99,
                Category = "Category B" },
                new Product(){
                Id = 6,
                ProductName = "Product 6 (Updated)",
                Description = "Description of product 6 (Updated)",
                Price = 60.99,
                Category = "Category C" },
                new Product(){
                Id = 7,
                ProductName = "Product 7 (Updated)",
                Description = "Description discount of product 7 (Updated)",
                Price = 70.99,
                Category = "Category A" },
                new Product(){
                Id = 8,
                ProductName = "Product 8 (Updated)",
                Description = "Description of product 8 (Updated)",
                Price = 80.99,
                Category = "Category B" },
                new Product(){
                Id = 9,
                ProductName = "Product 9 (Updated)",
                Description = "Description discount sale of product 9 (Updated)",
                Price = 90.99,
                Category = "Category C" },
                new Product(){
                Id = 10,
                ProductName = "Product 10 (Updated)",
                Description = "Description of product 10 (Updated)",
                Price = 100.99,
                Category = "Category A" },

            };
            return products;
        }
    }
}
