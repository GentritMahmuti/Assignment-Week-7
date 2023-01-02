using Elasticsearch.Net;
using lifeEcommerce.Helpers;
using Nest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var node = new SingleNodeConnectionPool(new Uri("https://localhost:9200"));
var connectionSettings = new ConnectionSettings(node);

var client = new ElasticClient(connectionSettings);
builder.Services.AddSingleton(client);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
