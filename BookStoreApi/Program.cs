using BookStoreApi.Data;
using BookStoreApi.Slices.Books.CreateBook;
using BookStoreApi.Slices.Books.DeleteBook;
using BookStoreApi.Slices.Books.GetBook;
using BookStoreApi.Slices.Books.GetBooks;
using BookStoreApi.Slices.Books.UpdateBook;
using BookStoreApi.Slices.Books.UploadCoverImage;
using BookStoreApi.Slices.Reviews.CreateReview;
using BookStoreApi.Slices.Reviews.GetReviews;
using BookStoreApi.Slices.Users.Login;
using BookStoreApi.Slices.Users.Register;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = "BookStoreApi",
        ValidateAudience = true,
        ValidAudience = "BookStoreApi",
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secret_password_jwt")),
        ValidateIssuerSigningKey = true
    };
});

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

// endpoints-auth
app.MapLoginEndpoint();
app.MapRegisterEndpoint();

// middleware 
//app.UseMiddleware<GlobalExceptionMiddleware>();

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

