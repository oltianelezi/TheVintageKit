using backend.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Register repository with dependency injection
builder.Services.AddScoped<ProductRepository>();

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
