using DDDExample.API.Settings;
using DDDExample.Application.Interfaces;
using DDDExample.Application.Mappings;
using DDDExample.Application.Services;
using DDDExample.Domain.Entities;
using DDDExample.Domain.Repositories;
using DDDExample.Infrastructure.Persistence;
using DDDExample.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;


var builder = WebApplication.CreateBuilder(args);
// Configurar serialización de GUID para MongoDB
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

// Agregar controladores
builder.Services.AddControllers();

// Configuraci�n de MongoDB
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    var settings = builder.Configuration.GetSection("MongoDB").Get<MongoDBSettings>();
    var mongoClientSettings = MongoClientSettings.FromUrl(
        new MongoUrl(settings.ConnectionString));
    mongoClientSettings.ServerApi = new ServerApi(ServerApiVersion.V1);
    return new MongoClient(mongoClientSettings);
});

builder.Services.AddScoped<IMongoDatabase>(serviceProvider =>
{
    var client = serviceProvider.GetRequiredService<IMongoClient>();
    var settings = serviceProvider.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    return client.GetDatabase(settings.DatabaseName);
});

// Registrar AutoMapper correctamente
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

// Registrar servicios de aplicaci�n y repositorios
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<MongoDBContext<Product>>(provider =>
    new MongoDBContext<Product>(
        provider.GetRequiredService<IMongoDatabase>(),
        "Products"));

// Configuraci�n de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DDDExample.API",
        Version = "v1",
        Description = "API basada en arquitectura DDD con MongoDB",
    });
});

var app = builder.Build();

// Configuraci�n de Swagger solo en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DDDExample.API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
