using BookStoreApi.Data;
using BookStoreApi.Slices.Books.CreateBook;
using BookStoreApi.Slices.Books.DeleteBook;
using BookStoreApi.Slices.Books.GetBook;
using BookStoreApi.Slices.Books.UpdateBook;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//db init
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options
    => options.UseNpgsql(connectionString));

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT bearer
builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = false;
});

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// endpoints-book
app.MapCreateBookEndpoint();
app.MapGetBookEndpoint();
app.MapUpdateBookEndpoint(); 
app.MapDeleteBookEndpoint();

// middleware 
//app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
//app.UseAuthorization();

app.Run();

