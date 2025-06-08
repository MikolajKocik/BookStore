using BookStoreApi.Data;
using BookStoreApi.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Slices.Users.Register
{
    public static class RegisterEndpoint
    {
        public static void MapRegisterEndpoint(this WebApplication app)
        {
            app.MapPost("/api/users/register", async (RegisterRequest request, AppDbContext context, IValidator<RegisterRequest> validator) =>
            {
                // validation
                var validationResult = validator.Validate(request);
                if(!validationResult.IsValid)
                {
                    return Results.BadRequest(string.Join(", ", "Validation failed", validationResult.Errors));
                }

                // user exist
                var userExist = await context.Users
                    .AnyAsync(u => u.UserName == request.UserName);
                if(userExist)
                {
                    return Results.Conflict("User already exists.");
                }

                // Hash password
                var hasher = new PasswordHasher<User>();
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = request.UserName
                };
                user.PasswordHash = hasher.HashPassword(user, request.Password);

                context.Users.Add(user);
                await context.SaveChangesAsync();

                return Results.Ok(new { user.Id, user.UserName });
            });
        }
    }
}
