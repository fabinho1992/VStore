using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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

//Authentication and Authorization 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],    // "vstore-auth"
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:ValidAudience"], // "vstore-apis"
            ValidateLifetime = true
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication(); // 👈👈👈 ESTA LINHA É OBRIGATÓRIA!

app.MapControllers();

app.Run();
