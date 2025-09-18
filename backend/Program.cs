using backend.Models;
using backend.Repositories;
using backend.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SupabaseDb");

builder.Services.AddControllers();

// Register repository with dependency injection
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<AddressRepository>();
builder.Services.AddScoped<OrderItemRepository>();
builder.Services.AddScoped<EmailService>();



// ---- ADD THIS: CORS POLICY ----
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()   // allow requests from any origin
              .AllowAnyHeader()   // allow any headers
              .AllowAnyMethod();  // allow GET/POST/etc.
    });
});

var app = builder.Build();

// ---- USE CORS ----
app.UseCors();

app.MapControllers();

app.Run();
