using backend.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Register repository with dependency injection
builder.Services.AddScoped<ProductRepository>();

var app = builder.Build();

app.MapControllers();

app.Run();
