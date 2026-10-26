
using FakeStore.Domain.Context;
using FakeStore.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Conexión con la base de datos PostgreSQL de FakeStore
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("FakeStoreConnection")));

// Servicios
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ProductDetailService>();
builder.Services.AddScoped<CategoriesService>();
builder.Services.AddScoped<AuthService>();

// Consumo de Fake Store API
builder.Services.AddHttpClient("FakeStore", client =>
{
    client.BaseAddress = new Uri("https://fakestoreapi.com/");
});

// Validación del token JWT
var claveJWT = builder.Configuration["KeyJWT"]
    ?? throw new InvalidOperationException("Falta configurar KeyJWT.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(claveJWT)),
            ClockSkew = TimeSpan.Zero
        };
    });

// Swagger: habilitar el botón Authorize
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
