using Assignment_Week_7.Models.DTOs;
using Assignment_Week_7.Models.Entities;
using Assignment_Week_7.Services;
using Assignment_Week_7.Services.IServices;
using Assignment_Week_7.Workers;
using Elasticsearch.Net;
using FluentValidation;
using lifeEcommerce.Helpers;
using lifeEcommerce.Validators;
using Nest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IValidator<SearchProductDto>, SerachProductDtoValidator>();
var node = new SingleNodeConnectionPool(new Uri("https://localhost:9200"));
var connectionSettings = new ConnectionSettings(node).BasicAuthentication("elastic", "A7mxMllCiAXLpnYBgqOF").CertificateFingerprint("734d384ab99183cd1d99ffb87c9e733ba8a681ffe2ce8b1836b4538a28787d9c");

var client = new ElasticClient(connectionSettings);
builder.Services.AddSingleton(client);

builder.Services.AddSingleton<UpdateProductsInElasticBackgroundWorker>();
builder.Services.AddHostedService<UpdateProductsInElasticBackgroundWorker>();
builder.Services.AddTransient<IProductService, ProductService>();

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
