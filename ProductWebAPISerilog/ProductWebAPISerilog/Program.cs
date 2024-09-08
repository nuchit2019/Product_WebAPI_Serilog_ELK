using Microsoft.Data.SqlClient;
using ProductWebAPISerilog.Repository;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
    {
        AutoRegisterTemplate = true,
        IndexFormat = "product-api-logs-{0:yyyy.MM.dd}",
        ModifyConnectionSettings = x => x.BasicAuthentication("elastic", "your_password")
    })
    .CreateLogger();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));


// Register the ProductRepository
builder.Services.AddTransient<ProductRepository>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
