using Reactapp.Interfaces;
using Reactapp.Services;
using System.Data;
using Microsoft.Data.SqlClient;
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register email service
builder.Services.AddScoped<IEmailservices, EmailService>();
builder.Services.AddScoped<IUserservices, UserServices>();
builder.Services.AddScoped<ILoginServices, LoginServices>();

builder.Services.AddScoped<IProductServices, ProductService>();
// Configure CORS (only once)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3002") // React app URL
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
builder.Services.AddScoped<IDbConnection>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("DefaultConnection");
    return new SqlConnection(connectionString);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseHttpsRedirection();
// Enable serving static files (like images)
app.UseStaticFiles();


// Enable CORS (must be before Authorization)
app.UseCors("AllowReactApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
