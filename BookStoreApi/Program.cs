using BookStoreApi.Data;
using BookStoreApi.Middleware;
using BookStoreApi.Slices.Books.CreateBook;
using BookStoreApi.Slices.Books.DeleteBook;
using BookStoreApi.Slices.Books.GetBook;
using BookStoreApi.Slices.Books.GetBooks;
using BookStoreApi.Slices.Books.UpdateBook;
using BookStoreApi.Slices.Books.UploadCoverImage;
using BookStoreApi.Slices.Reviews.CreateReview;
using BookStoreApi.Slices.Reviews.DeleteReview;
using BookStoreApi.Slices.Reviews.GetReviews;
using BookStoreApi.Slices.Reviews.UpdateReview;
using BookStoreApi.Slices.Users.Login;
using BookStoreApi.Slices.Users.Register;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// user secrets + appsettings also environement variable config
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();

// serilog
builder.Host.UseSerilog((context, logger) => logger
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Debug());

//db init
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.EnableSensitiveDataLogging();
});

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT bearer
var jwtSettings = builder.Configuration.GetSection("Jwt"); // from apssettings.json
var secretKey = jwtSettings["Key"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = audience,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey ??
            throw new ArgumentException("Jwt key not found"))),
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// endpoints-book
app.MapCreateBookEndpoint();
app.MapGetBookEndpoint();
app.MapGetBooksEndpoint();
app.MapUpdateBookEndpoint(); 
app.MapDeleteBookEndpoint();

// endpoint-coverImage
app.MapUploadCoverImageEndpoint();

// endpoints-review
app.MapCreateReviewEndpoint();
app.MapGetReviewsEndopint();
app.MapUpdateReviewEndpoint();
app.MapDeleteReviewEndpoint();

// endpoints-auth
app.MapLoginEndpoint();
app.MapRegisterEndpoint();

// middleware 
app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Run();

