using BookReviewManager.Infrastructure.Service.Identity;
using ClinicManagement.Infrastructure.Services.AuthService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserApi.Application.DependencysInjections;
using UserApi.Domain;
using UserApi.Domain.Interfaces.IAuthService;
using UserApi.Domain.Interfaces.IRepository;
using UserApi.Infrastructure;
using UserApi.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Injeção de dependencia
builder.Services.AddScoped<ICreateUser, CreateUser>();
builder.Services.AddScoped<ICreateRole, CreateRole>();
builder.Services.AddScoped<ILoginUser, LoginUser>();
builder.Services.AddScoped<IAddRole, AddRole>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IRepositoryUser, RepositoryUser>();


//JWT Token
var secretKey = builder.Configuration["Jwt:SecretKey"] ?? throw new ArgumentException("Invalid secret Key ..");

builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // desafio de solicitar o token
    }).AddJwtBearer(opt =>
    {
        opt.SaveToken = true; // salvar o token
        opt.RequireHttpsMetadata = true; // se é preciso https para transmitir o token , em produçao é true
        opt.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            ValidAudience = builder.Configuration["Jwt:ValidAudience"],
            ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))

        };
    });

//Politicas que serão usadas para acessar os endpoints
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("Funcionario", policy => policy.RequireRole("Funcionario"));

    opt.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
}
);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<User, IdentityRole>()
        .AddEntityFrameworkStores<UserDbContext>()
    .AddDefaultTokenProviders();

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
