using Assignment_Week_7.Models.Entities;
using Nest;

namespace Assignment_Week_7.Workers
{
    public class UpdateProductsInElsasticBackgroundWorker : IHostedService
    {
        private readonly IElasticClient _elasticClient;
        private Timer _timer;

        public UpdateProductsInElsasticBackgroundWorker(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdateIndex, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
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

        private async void UpdateIndex(object state)
        {
            // Retrieve new product data from some source (e.g. database, API)
            var newProductData = GetNewProductData();

            // Use the Elasticsearch bulk API to efficiently update the index
            var bulkResponse = await _elasticClient.BulkAsync(b => b
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
                new Product()
                {
                    Id = 1,
                    ProductName = "Product 1",
                    Description = "Description of product 1",
                    Price = 12.99,
                    Category = "Category A"
                },
                new Product()
                {
                    Id = 2,
                    ProductName = "Product 2",
                    Description = "Description of product 2",
                    Price = 22.99,
                    Category = "Category B"
                }
            };
            return products;
        }
    }
}
