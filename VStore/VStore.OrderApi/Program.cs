using Microsoft.EntityFrameworkCore;
using UserApi.Application.DependencysInjections;
using VStore.OrderApi.Apllication_Order.Dtos.Inputs;
using VStore.OrderApi.Apllication_Order.Dtos.Response;
using VStore.OrderApi.Apllication_Order.Mapping;
using VStore.OrderApi.Apllication_Order.ProductHttpClient;
using VStore.OrderApi.Apllication_Order.Service;
using VStore.OrderApi.Domain.IRepository;
using VStore.OrderApi.Domain.IService;
using VStore.OrderApi.Domain.Models;
using VStore.OrderApi.Infrastructure.OrderContext;
using VStore.OrderApi.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DbOrderContext>(options =>
    options.UseSqlServer(connectionString));
//Dependency Injection

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

//HttpClient 
builder.Services.AddHttpClient(); 
builder.Services.AddScoped<IHttpGetProducts, HttpGetProducts>(); 

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IRepositoryOrder<Order>, OrderReposiitory>();
builder.Services.AddScoped<ICRUDService<OrderResponse, OrderInput>, OrderService>();

builder.Services.AddMessageBus(builder.Configuration);

var app = builder.Build();

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
