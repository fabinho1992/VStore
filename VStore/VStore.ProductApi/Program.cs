using Microsoft.EntityFrameworkCore;
using VStore.ProductApi.Application.Dtos.Inputs;
using VStore.ProductApi.Application.Dtos.Responses;
using VStore.ProductApi.Application.Service;
using VStore.ProductApi.Domain.IRepository;
using VStore.ProductApi.Domain.IService;
using VStore.ProductApi.Domain.Models;
using VStore.ProductApi.Infrastructure.ProductContex;
using VStore.ProductApi.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DbProductContext>(options =>
    options.UseSqlServer(connectionString));

//Dependency Injection
builder.Services.AddScoped(typeof(IRepository<Product>), typeof(ProductRepository));
builder.Services.AddScoped(typeof(ICRUDService<ProductResponse, ProductInput>), typeof(ProductService));


builder.Services.AddScoped<IRepository<Category>, CategoryRepository>();
builder.Services.AddScoped<ICRUDService<CategoryResponse, CategoryInput>, CategoryService>();

//AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
