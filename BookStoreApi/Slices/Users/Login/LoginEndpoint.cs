using BookStoreApi.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStoreApi.Slices.Users.Login
{
    public static class LoginEndpoint
    {
        public static void MapLoginEndpoint(this WebApplication app)
        {
            app.MapPost("/api/users/login", async (LoginRequest request, AppDbContext context, IValidator<LoginRequest> validator) =>
            {
                // validation
                var validationResult = validator.Validate(request);
                if(!validationResult.IsValid)
                {
                    return Results.BadRequest(string.Join(", ", "Validation failed", validationResult.Errors));
                }

                // user exist?
                var user = await context.Users
                    .FirstOrDefaultAsync(u => u.UserName == request.UserName);
                if(user is null)
                {
                    return Results.NotFound("User not found");
                }

                // check password 
                if(user.PasswordHash != request.Password)
                {
                    return Results.Unauthorized();
                }

                // create token JWT
                var claims = new[]
                {              
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                // key / credentials / token
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secret_password_jwt"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "BookStoreApi",
                    audience: "BookStoreApi",
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: creds
                    );

                // token string
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Results.Ok(new {  Token = tokenString });
            });
        }
    }
}
